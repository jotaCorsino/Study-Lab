namespace StudyLab.Domain.Study;

public sealed record Progress(TimeSpan WatchedDuration, bool IsCompleted);
