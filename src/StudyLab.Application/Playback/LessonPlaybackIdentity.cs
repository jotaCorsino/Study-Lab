using System.Security.Cryptography;
using System.Text;
using StudyLab.Application.Common;

namespace StudyLab.Application.Playback;

public static class LessonPlaybackIdentity
{
    public static Guid FromCourseAndRelativePath(Guid courseId, string relativePath)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        string normalizedRelativePath = NormalizeRelativePath(relativePath).ToUpperInvariant();
        byte[] input = Encoding.UTF8.GetBytes($"{courseId:N}|{normalizedRelativePath}");
        byte[] hash = SHA256.HashData(input);
        byte[] guidBytes = new byte[16];

        Array.Copy(hash, guidBytes, guidBytes.Length);

        return new Guid(guidBytes);
    }

    internal static string NormalizeRelativePath(string relativePath)
    {
        string normalized = ApplicationGuard.RequiredText(relativePath, nameof(relativePath))
            .Replace('\\', '/');

        if (Path.IsPathRooted(normalized) || normalized.Split('/').Any(segment => segment == ".."))
        {
            throw new ArgumentException("Relative path must stay inside the stored course root.", nameof(relativePath));
        }

        return normalized;
    }
}
