using Microsoft.Extensions.Logging;
using POSAGENT.Domain.Files;

namespace POSAGENT.Infrastructure.FileSystem;

/// <summary>
/// Waits until a file can be opened exclusively (finished writing).
/// </summary>
public sealed class ExclusiveOpenFileStabilizer : IFileStabilizer
{
    private readonly ILogger<ExclusiveOpenFileStabilizer> _logger;
    private readonly int _maxAttempts;
    private readonly TimeSpan _delayBetweenAttempts;

    public ExclusiveOpenFileStabilizer(
        ILogger<ExclusiveOpenFileStabilizer> logger,
        int maxAttempts = 10,
        int delayMilliseconds = 500)
    {
        _logger = logger;
        _maxAttempts = maxAttempts;
        _delayBetweenAttempts = TimeSpan.FromMilliseconds(delayMilliseconds);
    }

    public async Task<bool> WaitUntilStableAsync(WatchedFile file, CancellationToken ct)
    {
        for (var i = 0; i < _maxAttempts; i++)
        {
            try
            {
                using var fs = new FileStream(file.FullPath, FileMode.Open, FileAccess.Read, FileShare.None);
                _logger.LogInformation("File {File} is stable after {Attempt} attempts.", file.FullPath, i + 1);
                return true;
            }
            catch (IOException)
            {
                _logger.LogDebug("File {File} is still locked, attempt {Attempt}/{Max}.", file.FullPath, i + 1, _maxAttempts);
                if (i < _maxAttempts - 1)
                    await Task.Delay(_delayBetweenAttempts, ct);
            }
        }

        _logger.LogWarning("File {File} did not become stable after {Max} attempts.", file.FullPath, _maxAttempts);
        return false;
    }
}
