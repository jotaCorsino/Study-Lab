using StudyLab.Domain.Study;

namespace StudyLab.Domain.Tests.Study;

public sealed class MonthlyCreditTests
{
    [Fact]
    public void ApplyAutomaticAbatementsUsesExtraTimeToCompensatePreviousPendingDays()
    {
        MonthlyCredit month = MonthlyCredit.For(2026, 4);

        month.RecordStudy(new DateOnly(2026, 4, 1), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(30));
        month.RecordStudy(new DateOnly(2026, 4, 2), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(90));

        month.ApplyAutomaticAbatements(new DateOnly(2026, 4, 2));

        StudyDayPerformance day = month.GetDay(new DateOnly(2026, 4, 1));
        Assert.Equal(TimeSpan.FromMinutes(30), day.CompensatedDuration);
        Assert.Equal(0.5m, day.OriginalPercentComplete);
        Assert.True(day.IsMetAfterCompensation);
        Assert.True(day.HasAbatementMarker);
        Assert.Equal(TimeSpan.Zero, month.GetRemainingCredit(new DateOnly(2026, 4, 2)));
    }

    [Fact]
    public void ApplyAutomaticAbatementsDoesNotCompensateCurrentDay()
    {
        MonthlyCredit month = MonthlyCredit.For(2026, 4);

        month.RecordStudy(new DateOnly(2026, 4, 1), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(120));
        month.RecordStudy(new DateOnly(2026, 4, 2), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(30));

        month.ApplyAutomaticAbatements(new DateOnly(2026, 4, 2));

        StudyDayPerformance currentDay = month.GetDay(new DateOnly(2026, 4, 2));
        Assert.Equal(TimeSpan.Zero, currentDay.CompensatedDuration);
        Assert.False(currentDay.IsMetAfterCompensation);
    }

    [Fact]
    public void RecordStudyRejectsDatesOutsideTrackedMonth()
    {
        MonthlyCredit month = MonthlyCredit.For(2026, 4);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            month.RecordStudy(new DateOnly(2026, 5, 1), TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(60)));
    }
}
