using StudyLab.Application.Common;

namespace StudyLab.Application.Courses.Importing;

public sealed class ImportCourseToLibraryCommand
{
    public ImportCourseToLibraryCommand(string rootPath, Guid courseId, DateTimeOffset importedAt)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        RootPath = ApplicationGuard.RequiredText(rootPath, nameof(rootPath));
        CourseId = courseId;
        ImportedAt = importedAt;
    }

    public string RootPath { get; }

    public Guid CourseId { get; }

    public DateTimeOffset ImportedAt { get; }
}
