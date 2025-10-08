namespace POSAGENT.Common.Options;

public sealed class WatchOptions
{
    public string Path { get; set; } = @"D:\ScanningDocuments";
    public string Filter { get; set; } = "*.*";
    public bool IncludeSubdirectories { get; set; } = false;
}
