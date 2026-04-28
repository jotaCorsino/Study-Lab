namespace StudyLab.Infrastructure.Courses.Importing;

public static class SafeRelativePath
{
    public static string From(string rootPath, string candidatePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(candidatePath);

        string fullRootPath = Path.GetFullPath(rootPath);
        string fullCandidatePath = Path.GetFullPath(candidatePath);
        string rootWithSeparator = EnsureTrailingSeparator(fullRootPath);

        if (!fullCandidatePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Path must stay inside the selected course root.");
        }

        string relativePath = Path.GetRelativePath(fullRootPath, fullCandidatePath).Replace('\\', '/');

        if (Path.IsPathRooted(relativePath) || relativePath.Split('/').Any(segment => segment == ".."))
        {
            throw new UnauthorizedAccessException("Path must stay inside the selected course root.");
        }

        return relativePath;
    }

    private static string EnsureTrailingSeparator(string path)
    {
        return path.EndsWith(Path.DirectorySeparatorChar)
            ? path
            : path + Path.DirectorySeparatorChar;
    }
}
