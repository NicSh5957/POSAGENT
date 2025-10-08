using POSAGENT.Domain.Files;
using POSAGENT.Domain.Pipeline;

namespace POSAGENT.Domain;

/// <summary>
/// Decides which pipeline should process a given file.
/// </summary>
public interface IFileRouter
{
    /// <summary>
    /// Returns the appropriate pipeline for the specified file.
    /// </summary>
    IFilePipeline Resolve(WatchedFile file);
}
