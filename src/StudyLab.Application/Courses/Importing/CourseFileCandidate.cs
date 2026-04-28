using StudyLab.Application.Common;

namespace StudyLab.Application.Courses.Importing;

public sealed record CourseFileCandidate
{
    public CourseFileCandidate(string relativePath)
    {
        RelativePath = RelativeCoursePath.Normalize(relativePath);
        Title = Path.GetFileNameWithoutExtension(RelativePath);
    }

    public string RelativePath { get; }

    public string Title { get; }
}
