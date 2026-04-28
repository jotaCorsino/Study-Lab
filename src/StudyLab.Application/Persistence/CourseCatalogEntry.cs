using StudyLab.Application.Common;

namespace StudyLab.Application.Persistence;

public sealed class CourseCatalogEntry
{
    public CourseCatalogEntry(
        Guid id,
        string title,
        string rootPath,
        IEnumerable<CourseCatalogItem> items,
        DateTimeOffset importedAt)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(id));
        }

        Id = id;
        Title = ApplicationGuard.RequiredText(title, nameof(title));
        RootPath = ApplicationGuard.RequiredText(rootPath, nameof(rootPath));
        Items = items?.ToArray() ?? throw new ArgumentNullException(nameof(items));
        ImportedAt = importedAt;
    }

    public Guid Id { get; }

    public string Title { get; }

    public string RootPath { get; }

    public IReadOnlyList<CourseCatalogItem> Items { get; }

    public DateTimeOffset ImportedAt { get; }
}
