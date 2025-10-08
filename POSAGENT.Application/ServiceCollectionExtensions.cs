using Microsoft.Extensions.DependencyInjection;

namespace POSAGENT.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPosagentApplication(this IServiceCollection services)
    {
        services.AddHostedService<Orchestration.WatcherOrchestrator>();

        return services;
    }
}
