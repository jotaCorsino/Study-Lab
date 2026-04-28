namespace StudyLab.Application.Courses.Importing;

public sealed class ImportedCourse
{
    private ImportedCourse(string title, IEnumerable<ImportedCourseItem> items)
    {
        Title = Common.ApplicationGuard.RequiredText(title, nameof(title));
        Items = items.ToArray();
    }

    public string Title { get; }

    public IReadOnlyList<ImportedCourseItem> Items { get; }

    public static ImportedCourse FromSnapshot(CourseFolderSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        ImportTreeBuilder builder = new();

        foreach (CourseFileCandidate videoFile in snapshot.VideoFiles
            .OrderBy(videoFile => videoFile.RelativePath, StringComparer.OrdinalIgnoreCase))
        {
            builder.Add(videoFile);
        }

        return new ImportedCourse(snapshot.CourseTitle, builder.Items);
    }
}
