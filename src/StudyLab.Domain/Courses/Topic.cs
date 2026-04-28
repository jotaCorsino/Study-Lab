using StudyLab.Domain.Common;

namespace StudyLab.Domain.Courses;

public sealed class Topic
{
    private readonly List<Lesson> _lessons = [];

    private Topic(string title)
    {
        Title = Guard.RequiredText(title, nameof(title));
    }

    public string Title { get; }

    public IReadOnlyList<Lesson> Lessons => _lessons;

    public static Topic Create(string title)
    {
        return new Topic(title);
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        _lessons.Add(lesson);
    }
}
