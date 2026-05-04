using StudyLab.Application.Persistence;

namespace StudyLab.Application.Courses.Importing;

public sealed class CourseLibraryImportResult
{
    public CourseLibraryImportResult(
        CourseCatalogEntry course,
        IEnumerable<RejectedCourseFile> rejectedFiles,
        CourseLibraryImportStatus status = CourseLibraryImportStatus.Imported)
    {
        Course = course ?? throw new ArgumentNullException(nameof(course));
        RejectedFiles = rejectedFiles?.ToArray() ?? throw new ArgumentNullException(nameof(rejectedFiles));
        Status = status;
    }

    public CourseCatalogEntry Course { get; }

    public IReadOnlyList<RejectedCourseFile> RejectedFiles { get; }

    public CourseLibraryImportStatus Status { get; }

    public bool WasImported => Status == CourseLibraryImportStatus.Imported;
}
