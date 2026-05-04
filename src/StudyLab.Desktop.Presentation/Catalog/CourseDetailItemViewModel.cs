using global::StudyLab.Application.Persistence;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class CourseDetailItemViewModel
{
    private CourseDetailItemViewModel(string title, string kindText, string iconGlyph, IEnumerable<CourseDetailItemViewModel> children)
    {
        Title = title;
        KindText = kindText;
        IconGlyph = iconGlyph;
        Children = children.ToArray();
    }

    public string Title { get; }

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
                item.Children.Select(FromDetailItem)),
            CourseCatalogItemType.Lesson => new CourseDetailItemViewModel(
                item.Title,
                "Aula",
                "\uE768",
                item.Children.Select(FromDetailItem)),
            _ => throw new InvalidOperationException("Course detail item type is not supported.")
        };
    }
}
