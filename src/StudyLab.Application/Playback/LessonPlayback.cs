using StudyLab.Application.Common;

namespace StudyLab.Application.Playback;

public sealed class LessonPlayback
{
    public LessonPlayback(
        Guid courseId,
        Guid lessonId,
        string courseTitle,
        string lessonTitle,
        string mediaPath)
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
        CourseTitle = ApplicationGuard.RequiredText(courseTitle, nameof(courseTitle));
        LessonTitle = ApplicationGuard.RequiredText(lessonTitle, nameof(lessonTitle));
        MediaPath = ApplicationGuard.RequiredText(mediaPath, nameof(mediaPath));
    }

    public Guid CourseId { get; }

    public Guid LessonId { get; }

    public string CourseTitle { get; }

    public string LessonTitle { get; }

    public string MediaPath { get; }
}
