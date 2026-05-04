using StudyLab.Application.Persistence;

namespace StudyLab.Application.Courses.Importing;

public sealed class ImportCourseToLibraryUseCase
{
    private readonly ImportCourseFromFolderUseCase _importCourseFromFolder;
    private readonly IStudyLibraryRepository _repository;

    public ImportCourseToLibraryUseCase(
        ImportCourseFromFolderUseCase importCourseFromFolder,
        IStudyLibraryRepository repository)
    {
        _importCourseFromFolder = importCourseFromFolder ?? throw new ArgumentNullException(nameof(importCourseFromFolder));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public CourseLibraryImportResult Import(ImportCourseToLibraryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        StudyLibrarySnapshot currentSnapshot = _repository.Load();
        CourseCatalogEntry? existingCourse = currentSnapshot.Courses
            .FirstOrDefault(course => HasSameRootPath(course.RootPath, command.RootPath));
        if (existingCourse is not null)
        {
            return new CourseLibraryImportResult(
                existingCourse,
                [],
                CourseLibraryImportStatus.DuplicateSkipped);
        }

        CourseImportResult importResult = _importCourseFromFolder.Import(new ImportCourseCommand(command.RootPath));
        CourseCatalogEntry course = new(
            command.CourseId,
            importResult.Course.Title,
            command.RootPath,
            importResult.Course.Items.Select(ToCatalogItem),
            command.ImportedAt);

        StudyLibrarySnapshot updatedSnapshot = new(
            currentSnapshot.Courses.Concat([course]),
            currentSnapshot.Progress,
            currentSnapshot.Preferences);

        _repository.Save(updatedSnapshot);

        return new CourseLibraryImportResult(course, importResult.RejectedFiles);
    }

    private static CourseCatalogItem ToCatalogItem(ImportedCourseItem item)
    {
        CourseCatalogItemType type = item.Type switch
        {
            ImportedCourseItemType.Folder => CourseCatalogItemType.Folder,
            ImportedCourseItemType.Lesson => CourseCatalogItemType.Lesson,
            _ => throw new InvalidOperationException("Imported course item type is not supported.")
        };

        return new CourseCatalogItem(
            type,
            item.Title,
            item.RelativePath,
            item.Children.Select(ToCatalogItem));
    }

    private static bool HasSameRootPath(string left, string right)
    {
        return string.Equals(
            NormalizeRootPath(left),
            NormalizeRootPath(right),
            StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeRootPath(string rootPath)
    {
        return Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootPath))
            .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }
}
