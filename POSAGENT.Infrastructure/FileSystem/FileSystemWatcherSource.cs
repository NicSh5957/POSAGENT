using Microsoft.Extensions.Logging;
using POSAGENT.Domain.Files;

namespace POSAGENT.Infrastructure.FileSystem;

/// <summary>
/// FileSystemWatcher-based implementation of IFileEventSource.
/// </summary>
public sealed class FileSystemWatcherSource : IFileEventSource
{
    private readonly FileSystemWatcher _watcher;
    private readonly ILogger<FileSystemWatcherSource> _logger;

    public event Func<WatchedFile, Task>? CreatedOrChanged;

    public FileSystemWatcherSource(
        string path,
        string filter,
        bool includeSubdirectories,
        ILogger<FileSystemWatcherSource> logger)
    {
        _logger = logger;
        Directory.CreateDirectory(path);

        _watcher = new FileSystemWatcher(path, filter)
        {
            IncludeSubdirectories = includeSubdirectories,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
        };

        _watcher.Created += OnEvent;
        _watcher.Changed += OnEvent;
        _watcher.Renamed += (s, e) => OnEvent(s, e);
        _watcher.Error += (s, e) => _logger.LogError(e.GetException(), "FileSystemWatcher error");
    }

    public Task StartAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting FileSystemWatcher...");
        _watcher.EnableRaisingEvents = true;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken ct)
    {
        _logger.LogInformation("Stopping FileSystemWatcher...");
        _watcher.EnableRaisingEvents = false;
        return Task.CompletedTask;
    }

    private void OnEvent(object? sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("FileSystemWatcher event: {File}", e.FullPath);
        if (CreatedOrChanged is null) return;

        var fi = new FileInfo(e.FullPath);
        var file = new WatchedFile(e.FullPath, fi.Name, fi.Exists ? fi.Length : null, fi.Exists ? fi.LastWriteTimeUtc : null);

        _ = CreatedOrChanged.Invoke(file);
    }

    public ValueTask DisposeAsync()
    {
        _watcher.Dispose();
        return ValueTask.CompletedTask;
    }
}
