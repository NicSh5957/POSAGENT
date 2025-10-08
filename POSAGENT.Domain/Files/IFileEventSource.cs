namespace POSAGENT.Domain.Files;

public interface IFileEventSource : IAsyncDisposable
{
    event Func<WatchedFile, Task> CreatedOrChanged;
    
    Task StartAsync(CancellationToken ct);

    Task StopAsync(CancellationToken ct);
}
