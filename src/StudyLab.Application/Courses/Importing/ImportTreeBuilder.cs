namespace StudyLab.Application.Courses.Importing;

internal sealed class ImportTreeBuilder
{
    private readonly List<ImportedCourseItem> _items = [];

    public IReadOnlyList<ImportedCourseItem> Items => _items;

    public void Add(CourseFileCandidate file)
    {
        ArgumentNullException.ThrowIfNull(file);

        string[] segments = file.RelativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            return;
        }

        if (segments.Length == 1)
        {
            _items.Add(ImportedCourseItem.Lesson(file));
            return;
        }

        ImportedCourseItem current = GetOrAddRootFolder(segments[0]);

        for (int index = 1; index < segments.Length - 1; index++)
        {
            current = current.GetOrAddFolder(segments[index]);
        }

        current.AddLesson(file);
    }

    private ImportedCourseItem GetOrAddRootFolder(string title)
    {
        ImportedCourseItem? existing = _items.FirstOrDefault(item =>
            item.Type == ImportedCourseItemType.Folder &&
            string.Equals(item.Title, title, StringComparison.OrdinalIgnoreCase));

        if (existing is not null)
        {
            return existing;
        }

        ImportedCourseItem folder = ImportedCourseItem.Folder(title);
        _items.Add(folder);
        return folder;
    }
}
