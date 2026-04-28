using StudyLab.Domain.Common;

namespace StudyLab.Domain.Study;

public sealed class StudyGoal
{
    public StudyGoal(TimeSpan targetDuration)
    {
        TargetDuration = Guard.PositiveDuration(targetDuration, nameof(targetDuration));
    }

    public TimeSpan TargetDuration { get; }

    public GoalProgress Evaluate(TimeSpan studiedDuration)
    {
        Guard.NonNegativeDuration(studiedDuration, nameof(studiedDuration));

        decimal percentComplete = (decimal)studiedDuration.Ticks / TargetDuration.Ticks;
        TimeSpan extraTime = studiedDuration > TargetDuration
            ? studiedDuration - TargetDuration
            : TimeSpan.Zero;

        return new GoalProgress(
            TargetDuration,
            studiedDuration,
            percentComplete,
            extraTime,
            studiedDuration >= TargetDuration);
    }
}
