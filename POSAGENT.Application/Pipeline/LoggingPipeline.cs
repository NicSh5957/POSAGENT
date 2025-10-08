using Microsoft.Extensions.Logging;
using POSAGENT.Domain.Files;
using POSAGENT.Domain.Pipeline;

namespace POSAGENT.Application.Pipeline;

/// <summary>
/// Minimal pipeline: just logs that a file is being processed.
/// </summary>
public sealed class LoggingPipeline : IFilePipeline
{
    private readonly ILogger<LoggingPipeline> _logger;

    public LoggingPipeline(ILogger<LoggingPipeline> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(WatchedFile file, CancellationToken ct)
    {
        _logger.LogInformation("Processing file: {Path} (size={Size}, lastWriteUtc={LastWriteUtc})",
            file.FullPath, file.SizeBytes, file.LastWriteUtc);
        
        return Task.CompletedTask;
    }
}
