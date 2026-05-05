using StudyLab.Application.Common;

namespace StudyLab.Application.Persistence;

public sealed class CourseDetailItem
{
    public CourseDetailItem(
        CourseCatalogItemType type,
        string title,
        string? relativePath,
        IEnumerable<CourseDetailItem> children,
        Guid? lessonId = null)
    {
        Type = type;
        Title = ApplicationGuard.RequiredText(title, nameof(title));
        RelativePath = relativePath;
        Children = children?.ToArray() ?? throw new ArgumentNullException(nameof(children));
        LessonId = NormalizeLessonId(type, lessonId);
    }

    public CourseCatalogItemType Type { get; }

    public Guid? LessonId { get; }

    public string Title { get; }

    public string? RelativePath { get; }

    public IReadOnlyList<CourseDetailItem> Children { get; }

    private static Guid? NormalizeLessonId(CourseCatalogItemType type, Guid? lessonId)
    {
        if (lessonId == Guid.Empty)
        {
            throw new ArgumentException("Lesson id cannot be empty.", nameof(lessonId));
        }

        if (type == CourseCatalogItemType.Lesson)
        {
            return lessonId ?? throw new ArgumentException("Lesson item must have a lesson id.", nameof(lessonId));
        }

        if (lessonId.HasValue)
        {
            throw new ArgumentException("Only lesson items can have a lesson id.", nameof(lessonId));
        }

        return null;
    }
}
