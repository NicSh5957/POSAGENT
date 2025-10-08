using POSAGENT.Domain.Files;

namespace POSAGENT.Domain.Pipeline;

/// <summary>
/// Orchestrates execution of file processing steps for a given file.
/// </summary>
public interface IFilePipeline
{
    /// <summary>
    /// Runs the pipeline for the specified file.
    /// </summary>
    Task ProcessAsync(WatchedFile file, CancellationToken ct);
}
