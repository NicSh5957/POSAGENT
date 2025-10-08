using POSAGENT.Domain.Files;

namespace POSAGENT.Domain.Pipeline;

/// <summary>
/// Represents a single step in the file processing pipeline.
/// </summary>
public interface IFileProcessingStep
{
    /// <summary>
    /// Executes the step for the specified file.
    /// </summary>
    Task ExecuteAsync(FileContext context, CancellationToken ct);
}

/// <summary>
/// Context passed between pipeline steps.
/// </summary>
public sealed class FileContext
{
    public WatchedFile File { get; }
    public IDictionary<string, object?> Items { get; } = new Dictionary<string, object?>();

    public FileContext(WatchedFile file)
    {
        File = file;
    }
}
