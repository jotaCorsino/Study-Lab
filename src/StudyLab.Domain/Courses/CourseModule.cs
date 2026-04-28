using StudyLab.Domain.Common;

namespace StudyLab.Domain.Courses;

public sealed class CourseModule
{
    private readonly List<Topic> _topics = [];

    private CourseModule(string title)
    {
        Title = Guard.RequiredText(title, nameof(title));
    }

    public string Title { get; }

    public IReadOnlyList<Topic> Topics => _topics;

    public static CourseModule Create(string title)
    {
        return new CourseModule(title);
    }

    public void AddTopic(Topic topic)
    {
        ArgumentNullException.ThrowIfNull(topic);

        _topics.Add(topic);
    }
}
