using global::StudyLab.Application.Persistence;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class CourseDetailItemViewModel
{
    private CourseDetailItemViewModel(
        string title,
        string kindText,
        string iconGlyph,
        Guid? lessonId,
        IEnumerable<CourseDetailItemViewModel> children)
    {
        Title = title;
        KindText = kindText;
        IconGlyph = iconGlyph;
        LessonId = lessonId;
        Children = children.ToArray();
    }

    public string Title { get; }

    public Guid? LessonId { get; }

    public bool CanOpenLesson => LessonId.HasValue;

    public string KindText { get; }

    public string IconGlyph { get; }

    public IReadOnlyList<CourseDetailItemViewModel> Children { get; }

    public static CourseDetailItemViewModel FromDetailItem(CourseDetailItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return item.Type switch
        {
            CourseCatalogItemType.Folder => new CourseDetailItemViewModel(
                item.Title,
                "Modulo",
                "\uE8B7",
                null,
                item.Children.Select(FromDetailItem)),
            CourseCatalogItemType.Lesson => new CourseDetailItemViewModel(
                item.Title,
                "Aula",
                "\uE768",
                item.LessonId,
                item.Children.Select(FromDetailItem)),
            _ => throw new InvalidOperationException("Course detail item type is not supported.")
        };
    }
}
