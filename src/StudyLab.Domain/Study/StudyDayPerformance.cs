using StudyLab.Domain.Common;

namespace StudyLab.Domain.Study;

public sealed record StudyDayPerformance
{
    private StudyDayPerformance(
        DateOnly date,
        TimeSpan targetDuration,
        TimeSpan studiedDuration,
        TimeSpan compensatedDuration)
    {
        Date = date;
        TargetDuration = Guard.PositiveDuration(targetDuration, nameof(targetDuration));
        StudiedDuration = Guard.NonNegativeDuration(studiedDuration, nameof(studiedDuration));
        CompensatedDuration = Guard.NonNegativeDuration(compensatedDuration, nameof(compensatedDuration));
    }

    public DateOnly Date { get; }

    public TimeSpan TargetDuration { get; }

    public TimeSpan StudiedDuration { get; }

    public TimeSpan CompensatedDuration { get; }

    public decimal OriginalPercentComplete => (decimal)StudiedDuration.Ticks / TargetDuration.Ticks;

    public TimeSpan ExtraDuration => StudiedDuration > TargetDuration
        ? StudiedDuration - TargetDuration
        : TimeSpan.Zero;

    public TimeSpan RemainingDeficit
    {
        get
        {
            TimeSpan deficit = TargetDuration - StudiedDuration - CompensatedDuration;
            return deficit > TimeSpan.Zero ? deficit : TimeSpan.Zero;
        }
    }

    public bool IsMetAfterCompensation => StudiedDuration + CompensatedDuration >= TargetDuration;

    public bool HasAbatementMarker => CompensatedDuration > TimeSpan.Zero;

    public static StudyDayPerformance Record(DateOnly date, TimeSpan targetDuration, TimeSpan studiedDuration)
    {
        return new StudyDayPerformance(date, targetDuration, studiedDuration, TimeSpan.Zero);
    }

    public StudyDayPerformance WithAdditionalCompensation(TimeSpan compensation)
    {
        Guard.NonNegativeDuration(compensation, nameof(compensation));

        if (compensation > RemainingDeficit)
        {
            throw new ArgumentOutOfRangeException(nameof(compensation), compensation, "Compensation cannot exceed the remaining deficit.");
        }

        return new StudyDayPerformance(
            Date,
            TargetDuration,
            StudiedDuration,
            CompensatedDuration + compensation);
    }
}
