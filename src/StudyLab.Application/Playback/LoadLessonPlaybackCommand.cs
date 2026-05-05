namespace StudyLab.Application.Playback;

public sealed class LoadLessonPlaybackCommand
{
    public LoadLessonPlaybackCommand(Guid courseId, Guid lessonId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        if (lessonId == Guid.Empty)
        {
            throw new ArgumentException("Lesson id cannot be empty.", nameof(lessonId));
        }

        CourseId = courseId;
        LessonId = lessonId;
    }

    public Guid CourseId { get; }

    public Guid LessonId { get; }
}
