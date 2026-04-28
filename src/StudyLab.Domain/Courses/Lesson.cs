using StudyLab.Domain.Common;
using StudyLab.Domain.Study;

namespace StudyLab.Domain.Courses;

public sealed class Lesson
{
    private Lesson(LessonId id, string title, TimeSpan duration)
    {
        Id = id;
        Title = Guard.RequiredText(title, nameof(title));
        Duration = Guard.PositiveDuration(duration, nameof(duration));
    }

    public LessonId Id { get; }

    public string Title { get; }

    public TimeSpan Duration { get; }

    public static Lesson Create(string title, TimeSpan duration)
    {
        return new Lesson(LessonId.New(), title, duration);
    }
}
