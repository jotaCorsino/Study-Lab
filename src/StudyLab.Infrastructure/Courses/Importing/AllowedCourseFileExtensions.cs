using System.Collections.Frozen;

namespace StudyLab.Infrastructure.Courses.Importing;

internal static class AllowedCourseFileExtensions
{
    private static readonly FrozenSet<string> VideoExtensions = new[]
    {
        ".mp4",
        ".mkv",
        ".avi",
        ".mov",
        ".wmv",
        ".webm",
        ".flv",
        ".m4v"
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    public static bool IsVideo(string filePath)
    {
        return VideoExtensions.Contains(Path.GetExtension(filePath));
    }
}
