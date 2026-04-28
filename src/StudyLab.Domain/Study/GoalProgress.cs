namespace StudyLab.Domain.Study;

public sealed record GoalProgress(
    TimeSpan TargetDuration,
    TimeSpan StudiedDuration,
    decimal PercentComplete,
    TimeSpan ExtraTime,
    bool IsMet);
