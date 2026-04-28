namespace StudyLab.Application.Persistence;

public sealed class LessonProgressEntry
{
    public LessonProgressEntry(Guid lessonId, TimeSpan watchedDuration, bool isCompleted)
    {
        if (lessonId == Guid.Empty)
        {
            throw new ArgumentException("Lesson id cannot be empty.", nameof(lessonId));
        }

        if (watchedDuration < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(watchedDuration), watchedDuration, "Watched duration cannot be negative.");
        }

        LessonId = lessonId;
        WatchedDuration = watchedDuration;
        IsCompleted = isCompleted;
    }

    public Guid LessonId { get; }

    public TimeSpan WatchedDuration { get; }

    public bool IsCompleted { get; }
}
