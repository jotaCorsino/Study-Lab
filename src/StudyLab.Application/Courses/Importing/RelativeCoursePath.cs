using StudyLab.Application.Common;

namespace StudyLab.Application.Courses.Importing;

internal static class RelativeCoursePath
{
    public static string Normalize(string relativePath)
    {
        string normalized = ApplicationGuard.RequiredText(relativePath, nameof(relativePath))
            .Replace('\\', '/');

        if (Path.IsPathRooted(normalized) || normalized.Split('/').Any(segment => segment == ".."))
        {
            throw new ArgumentException("Relative path must stay inside the selected course root.", nameof(relativePath));
        }

        return normalized;
    }
}
