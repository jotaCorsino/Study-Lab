namespace StudyLab.Application.Courses.Importing;

public sealed class CourseFolderSnapshot
{
    public CourseFolderSnapshot(
        string courseTitle,
        IEnumerable<CourseFileCandidate> videoFiles,
        IEnumerable<RejectedCourseFile> rejectedFiles)
    {
        CourseTitle = Application.Common.ApplicationGuard.RequiredText(courseTitle, nameof(courseTitle));
        VideoFiles = videoFiles?.ToArray() ?? throw new ArgumentNullException(nameof(videoFiles));
        RejectedFiles = rejectedFiles?.ToArray() ?? throw new ArgumentNullException(nameof(rejectedFiles));
    }

    public string CourseTitle { get; }

    public IReadOnlyList<CourseFileCandidate> VideoFiles { get; }

    public IReadOnlyList<RejectedCourseFile> RejectedFiles { get; }
}
