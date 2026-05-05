namespace StudyLab.Desktop.Tests.Catalog;

public sealed class CourseDetailPageXamlTests
{
    [Fact]
    public void CourseTreeTemplateBindsToTreeViewNodeContent()
    {
        string xaml = File.ReadAllText(GetCourseDetailPagePath());

        Assert.Contains("Glyph=\"{Binding Content.IconGlyph}\"", xaml, StringComparison.Ordinal);
        Assert.Contains("Text=\"{Binding Content.Title}\"", xaml, StringComparison.Ordinal);
        Assert.Contains("Text=\"{Binding Content.KindText}\"", xaml, StringComparison.Ordinal);
    }

    private static string GetCourseDetailPagePath()
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "StudyLab.slnx")))
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException("Repository root was not found.");
        }

        return Path.Combine(directory.FullName, "src", "StudyLab.Desktop", "CourseDetailPage.xaml");
    }
}
