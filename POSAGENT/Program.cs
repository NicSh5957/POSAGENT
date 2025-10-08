using POSAGENT.Application;
using POSAGENT.Infrastructure;

namespace POSAGENT;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddWindowsService(o => o.ServiceName = "POSAGENT");

        builder.Services.AddPosagentInfrastructure(builder.Configuration);
        builder.Services.AddPosagentApplication();

        var host = builder.Build();
        host.Run();
    }
}
