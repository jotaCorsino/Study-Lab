using StudyLab.Application.Persistence;

namespace StudyLab.Application.Courses.Importing;

public sealed class CourseLibraryImportResult
{
    public CourseLibraryImportResult(CourseCatalogEntry course, IEnumerable<RejectedCourseFile> rejectedFiles)
    {
        Course = course ?? throw new ArgumentNullException(nameof(course));
        RejectedFiles = rejectedFiles?.ToArray() ?? throw new ArgumentNullException(nameof(rejectedFiles));
    }

    public CourseCatalogEntry Course { get; }

    public IReadOnlyList<RejectedCourseFile> RejectedFiles { get; }
}
