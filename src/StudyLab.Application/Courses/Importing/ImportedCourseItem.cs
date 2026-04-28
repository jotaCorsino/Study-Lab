namespace StudyLab.Application.Courses.Importing;

public sealed class ImportedCourseItem
{
    private readonly List<ImportedCourseItem> _children = [];

    private ImportedCourseItem(ImportedCourseItemType type, string title, string? relativePath)
    {
        Type = type;
        Title = Common.ApplicationGuard.RequiredText(title, nameof(title));
        RelativePath = relativePath;
    }

    public ImportedCourseItemType Type { get; }

    public string Title { get; }

    public string? RelativePath { get; }

    public IReadOnlyList<ImportedCourseItem> Children => _children;

    internal static ImportedCourseItem Folder(string title)
    {
        return new ImportedCourseItem(ImportedCourseItemType.Folder, title, relativePath: null);
    }

    internal static ImportedCourseItem Lesson(CourseFileCandidate file)
    {
        ArgumentNullException.ThrowIfNull(file);

        return new ImportedCourseItem(ImportedCourseItemType.Lesson, file.Title, file.RelativePath);
    }

    internal ImportedCourseItem GetOrAddFolder(string title)
    {
        ImportedCourseItem? existing = _children.FirstOrDefault(child =>
            child.Type == ImportedCourseItemType.Folder &&
            string.Equals(child.Title, title, StringComparison.OrdinalIgnoreCase));

        if (existing is not null)
        {
            return existing;
        }

        ImportedCourseItem folder = Folder(title);
        _children.Add(folder);
        return folder;
    }

    internal void AddLesson(CourseFileCandidate file)
    {
        _children.Add(Lesson(file));
    }
}
