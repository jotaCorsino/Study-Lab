namespace StudyLab.Application.Courses.Importing;

public interface ICourseFolderReader
{
    CourseFolderSnapshot Read(ImportCourseCommand command);
}
