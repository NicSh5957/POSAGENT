namespace POSAGENT.Domain.Files;

/// <summary>
/// Provides a way to wait until a file becomes stable
/// (finished writing and ready for processing).
/// </summary>
public interface IFileStabilizer
{
    /// <summary>
    /// Waits until the specified file is no longer being modified and can be read.
    /// Returns true if the file is stable, false if it failed to stabilize in time.
    /// </summary>
    Task<bool> WaitUntilStableAsync(WatchedFile file, CancellationToken ct);
}
