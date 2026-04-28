namespace StudyLab.Application.Courses.Importing;

public sealed class ImportCourseFromFolderUseCase
{
    private readonly ICourseFolderReader _reader;

    public ImportCourseFromFolderUseCase(ICourseFolderReader reader)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    }

    public CourseImportResult Import(ImportCourseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        CourseFolderSnapshot snapshot = _reader.Read(command);
        ImportedCourse course = ImportedCourse.FromSnapshot(snapshot);

        return new CourseImportResult(course, snapshot.RejectedFiles);
    }
}
