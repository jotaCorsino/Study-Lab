namespace StudyLab.Application.Playback;

public sealed class RecordLessonProgressCommand
{
    public RecordLessonProgressCommand(
        Guid courseId,
        Guid lessonId,
        TimeSpan watchedDuration,
        bool isCompleted)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        if (lessonId == Guid.Empty)
        {
            throw new ArgumentException("Lesson id cannot be empty.", nameof(lessonId));
        }

        if (watchedDuration < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(watchedDuration), watchedDuration, "Watched duration cannot be negative.");
        }

        CourseId = courseId;
        LessonId = lessonId;
        WatchedDuration = watchedDuration;
        IsCompleted = isCompleted;
    }

    public Guid CourseId { get; }

    public Guid LessonId { get; }

    public TimeSpan WatchedDuration { get; }

    public bool IsCompleted { get; }
}
