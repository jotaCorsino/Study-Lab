using StudyLab.Domain.Common;

namespace StudyLab.Domain.Courses;

public sealed class Course
{
    private readonly List<CourseModule> _modules = [];

    private Course(CourseId id, string title)
    {
        Id = id;
        Title = Guard.RequiredText(title, nameof(title));
    }

    public CourseId Id { get; }

    public string Title { get; }

    public IReadOnlyList<CourseModule> Modules => _modules;

    public static Course Create(string title)
    {
        return new Course(CourseId.New(), title);
    }

    public void AddModule(CourseModule module)
    {
        ArgumentNullException.ThrowIfNull(module);

        _modules.Add(module);
    }
}
