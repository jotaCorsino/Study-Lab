using StudyLab.Application.Common;

namespace StudyLab.Application.Courses.Importing;

public sealed class ImportCourseCommand
{
    public ImportCourseCommand(string rootPath)
    {
        RootPath = ApplicationGuard.RequiredText(rootPath, nameof(rootPath));
    }

    public string RootPath { get; }
}
