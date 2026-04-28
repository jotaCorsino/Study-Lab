using StudyLab.Application.Common;

namespace StudyLab.Application.Persistence;

public sealed class CourseCatalogItem
{
    public CourseCatalogItem(
        CourseCatalogItemType type,
        string title,
        string? relativePath,
        IEnumerable<CourseCatalogItem> children)
    {
        Type = type;
        Title = ApplicationGuard.RequiredText(title, nameof(title));
        RelativePath = NormalizeRelativePath(type, relativePath);
        Children = children?.ToArray() ?? throw new ArgumentNullException(nameof(children));
    }

    public CourseCatalogItemType Type { get; }

    public string Title { get; }

    public string? RelativePath { get; }

    public IReadOnlyList<CourseCatalogItem> Children { get; }

    private static string? NormalizeRelativePath(CourseCatalogItemType type, string? relativePath)
    {
        if (type == CourseCatalogItemType.Folder)
        {
            return relativePath is null
                ? null
                : NormalizePath(relativePath);
        }

        return NormalizePath(ApplicationGuard.RequiredText(relativePath ?? string.Empty, nameof(relativePath)));
    }

    private static string NormalizePath(string relativePath)
    {
        string normalized = relativePath.Replace('\\', '/');

        if (Path.IsPathRooted(normalized) || normalized.Split('/').Any(segment => segment == ".."))
        {
            throw new ArgumentException("Relative path must stay inside the stored course root.", nameof(relativePath));
        }

        return normalized;
    }
}
