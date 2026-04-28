using StudyLab.Domain.Study;

namespace StudyLab.Domain.Tests.Study;

public sealed class StudyGoalTests
{
    [Fact]
    public void EvaluateCalculatesPercentAndExtraTimeWhenTargetIsExceeded()
    {
        StudyGoal goal = new(TimeSpan.FromMinutes(60));

        GoalProgress progress = goal.Evaluate(TimeSpan.FromMinutes(90));

        Assert.Equal(1.5m, progress.PercentComplete);
        Assert.Equal(TimeSpan.FromMinutes(30), progress.ExtraTime);
        Assert.True(progress.IsMet);
    }

    [Fact]
    public void EvaluateRejectsNegativeStudiedTime()
    {
        StudyGoal goal = new(TimeSpan.FromMinutes(60));

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            goal.Evaluate(TimeSpan.FromMinutes(-1)));
    }

    [Fact]
    public void ConstructorRejectsZeroTarget()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new StudyGoal(TimeSpan.Zero));
    }
}
