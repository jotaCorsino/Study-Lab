namespace StudyLab.Application.Courses.Importing;

public sealed class CourseImportResult
{
    public CourseImportResult(ImportedCourse course, IEnumerable<RejectedCourseFile> rejectedFiles)
    {
        Course = course ?? throw new ArgumentNullException(nameof(course));
        RejectedFiles = rejectedFiles?.ToArray() ?? throw new ArgumentNullException(nameof(rejectedFiles));
    }

    public ImportedCourse Course { get; }

    public IReadOnlyList<RejectedCourseFile> RejectedFiles { get; }
}
