using global::StudyLab.Application.Persistence;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class CatalogCourseViewModel
{
    private CatalogCourseViewModel(Guid id, string title, int lessonCount)
    {
        Id = id;
        Title = title;
        LessonCount = lessonCount;
    }

    public Guid Id { get; }

    public string Title { get; }

    public int LessonCount { get; }

    public string LessonCountText => LessonCount == 1 ? "1 aula" : $"{LessonCount} aulas";

    public static CatalogCourseViewModel FromEntry(CourseCatalogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        return new CatalogCourseViewModel(
            entry.Id,
            entry.Title,
            CountLessons(entry.Items));
    }

    private static int CountLessons(IEnumerable<CourseCatalogItem> items)
    {
        return items.Sum(item =>
            item.Type == CourseCatalogItemType.Lesson
                ? 1
                : CountLessons(item.Children));
    }
}
