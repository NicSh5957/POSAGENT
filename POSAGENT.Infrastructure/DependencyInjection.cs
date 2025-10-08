using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using POSAGENT.Common.Options;
using POSAGENT.Domain.Files;
using POSAGENT.Domain;
using POSAGENT.Infrastructure.FileSystem;
using POSAGENT.Infrastructure.Routing;

namespace POSAGENT.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPosagentInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<WatchOptions>(config.GetSection("Watch"));
        
        services.AddSingleton<IFileEventSource>(sp =>
        {
            var watch = sp.GetRequiredService<IOptions<WatchOptions>>().Value;
            var logger = sp.GetRequiredService<ILogger<FileSystemWatcherSource>>();
            return new FileSystemWatcherSource(
                path: watch.Path,
                filter: watch.Filter,
                includeSubdirectories: watch.IncludeSubdirectories,
                logger: logger);
        });
        
        services.AddSingleton<IFileStabilizer, ExclusiveOpenFileStabilizer>();
        
        services.AddSingleton<IFileRouter, SinglePipelineRouter>();

        return services;
    }
}
