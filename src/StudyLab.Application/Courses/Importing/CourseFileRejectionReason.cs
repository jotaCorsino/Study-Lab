namespace StudyLab.Application.Courses.Importing;

public enum CourseFileRejectionReason
{
    UnsupportedExtension = 1,
    PathOutsideRoot = 2,
    ReparsePointNotAllowed = 3
}
