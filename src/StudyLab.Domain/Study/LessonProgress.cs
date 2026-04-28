using StudyLab.Domain.Common;

namespace StudyLab.Domain.Study;

public sealed class LessonProgress
{
    private LessonProgress(LessonId lessonId, TimeSpan lessonDuration)
    {
        LessonId = lessonId;
        LessonDuration = Guard.PositiveDuration(lessonDuration, nameof(lessonDuration));
        WatchedDuration = TimeSpan.Zero;
    }

    public LessonId LessonId { get; }

    public TimeSpan LessonDuration { get; }

    public TimeSpan WatchedDuration { get; private set; }

    public bool IsCompleted { get; private set; }

    public static LessonProgress Start(LessonId lessonId, TimeSpan lessonDuration)
    {
        return new LessonProgress(lessonId, lessonDuration);
    }

    public void RecordWatched(TimeSpan watchedDuration)
    {
        Guard.NonNegativeDuration(watchedDuration, nameof(watchedDuration));

        TimeSpan normalizedDuration = watchedDuration > LessonDuration
            ? LessonDuration
            : watchedDuration;

        if (normalizedDuration > WatchedDuration)
        {
            WatchedDuration = normalizedDuration;
        }

        if (WatchedDuration >= LessonDuration)
        {
            IsCompleted = true;
        }
    }

    public void MarkCompleted()
    {
        WatchedDuration = LessonDuration;
        IsCompleted = true;
    }
}
