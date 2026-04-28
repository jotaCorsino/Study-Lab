namespace StudyLab.Application.Courses.Importing;

public sealed record RejectedCourseFile(string RelativePath, CourseFileRejectionReason Reason)
{
    public string RelativePath { get; } = RelativeCoursePath.Normalize(RelativePath);
}
