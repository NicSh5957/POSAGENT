namespace POSAGENT.Domain.Files;

public sealed record WatchedFile(
    string FullPath,
    string Name,
    long? SizeBytes,
    DateTimeOffset? LastWriteUtc
);
