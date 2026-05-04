using StudyLab.Application.Common;

namespace StudyLab.Application.Persistence;

public sealed class CourseDetail
{
    public CourseDetail(
        Guid id,
        string title,
        IEnumerable<CourseDetailItem> items,
        DateTimeOffset importedAt)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(id));
        }

        Id = id;
        Title = ApplicationGuard.RequiredText(title, nameof(title));
        Items = items?.ToArray() ?? throw new ArgumentNullException(nameof(items));
        ImportedAt = importedAt;
        LessonCount = CountLessons(Items);
    }

    public Guid Id { get; }

    public string Title { get; }

    public IReadOnlyList<CourseDetailItem> Items { get; }

    public DateTimeOffset ImportedAt { get; }

    public int LessonCount { get; }

    private static int CountLessons(IEnumerable<CourseDetailItem> items)
    {
        return items.Sum(item =>
            item.Type == CourseCatalogItemType.Lesson
                ? 1
                : CountLessons(item.Children));
    }
}
