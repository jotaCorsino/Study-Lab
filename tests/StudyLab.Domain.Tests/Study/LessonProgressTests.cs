using StudyLab.Domain.Study;

namespace StudyLab.Domain.Tests.Study;

public sealed class LessonProgressTests
{
    [Fact]
    public void RecordWatchedCompletesLessonWhenFullDurationWasWatched()
    {
        LessonProgress progress = LessonProgress.Start(
            LessonId.New(),
            TimeSpan.FromMinutes(10));

        progress.RecordWatched(TimeSpan.FromMinutes(10));

        Assert.True(progress.IsCompleted);
        Assert.Equal(TimeSpan.FromMinutes(10), progress.WatchedDuration);
    }

    [Fact]
    public void RecordWatchedKeepsHighestWatchedDurationWhenPlayerReportsOlderPosition()
    {
        LessonProgress progress = LessonProgress.Start(
            LessonId.New(),
            TimeSpan.FromMinutes(10));

        progress.RecordWatched(TimeSpan.FromMinutes(8));
        progress.RecordWatched(TimeSpan.FromMinutes(3));

        Assert.Equal(TimeSpan.FromMinutes(8), progress.WatchedDuration);
        Assert.False(progress.IsCompleted);
    }

    [Fact]
    public void StartRejectsInvalidDuration()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            LessonProgress.Start(LessonId.New(), TimeSpan.Zero));
    }
}
