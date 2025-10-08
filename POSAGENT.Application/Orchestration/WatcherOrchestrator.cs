using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using POSAGENT.Domain;
using POSAGENT.Domain.Files;

namespace POSAGENT.Application.Orchestration;

/// <summary>
/// Background service that listens for file system events,
/// waits for file stabilization and runs the appropriate pipeline.
/// </summary>
public sealed class WatcherOrchestrator : BackgroundService
{
    private readonly IFileEventSource _source;
    private readonly IFileStabilizer _stabilizer;
    private readonly IFileRouter _router;
    private readonly ILogger<WatcherOrchestrator> _logger;

    public WatcherOrchestrator(
        IFileEventSource source,
        IFileStabilizer stabilizer,
        IFileRouter router,
        ILogger<WatcherOrchestrator> logger)
    {
        _source = source;
        _stabilizer = stabilizer;
        _router = router;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _source.CreatedOrChanged += async file =>
        {
            try
            {
                _logger.LogInformation("File event received: {File}", file.FullPath);

                var stable = await _stabilizer.WaitUntilStableAsync(file, stoppingToken);
                if (!stable)
                {
                    _logger.LogWarning("File {File} did not stabilize in time.", file.FullPath);
                    return;
                }

                var pipeline = _router.Resolve(file);
                await pipeline.ProcessAsync(file, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing file {File}", file.FullPath);
            }
        };

        _logger.LogInformation("Starting file watcher...");
        await _source.StartAsync(stoppingToken);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        finally
        {
            await _source.StopAsync(stoppingToken);
            await _source.DisposeAsync();
            _logger.LogInformation("File watcher stopped.");
        }
    }
}
