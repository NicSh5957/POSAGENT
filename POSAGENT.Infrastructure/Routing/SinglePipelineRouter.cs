using POSAGENT.Domain.Files;
using POSAGENT.Domain.Pipeline;
using POSAGENT.Domain;

namespace POSAGENT.Infrastructure.Routing;

/// <summary>
/// Always returns the same pipeline. Useful for initial wiring and tests.
/// </summary>
public sealed class SinglePipelineRouter : IFileRouter
{
    private readonly IFilePipeline _pipeline;

    public SinglePipelineRouter(IFilePipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public IFilePipeline Resolve(WatchedFile file) => _pipeline;
}
