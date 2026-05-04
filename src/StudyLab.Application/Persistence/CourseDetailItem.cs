using StudyLab.Application.Common;

namespace StudyLab.Application.Persistence;

public sealed class CourseDetailItem
{
    public CourseDetailItem(
        CourseCatalogItemType type,
        string title,
        string? relativePath,
        IEnumerable<CourseDetailItem> children)
    {
        Type = type;
        Title = ApplicationGuard.RequiredText(title, nameof(title));
        RelativePath = relativePath;
        Children = children?.ToArray() ?? throw new ArgumentNullException(nameof(children));
    }

    public CourseCatalogItemType Type { get; }

    public string Title { get; }

    public string? RelativePath { get; }

    public IReadOnlyList<CourseDetailItem> Children { get; }
}
