using StudyLab.Application.Courses.Importing;

namespace StudyLab.Infrastructure.Courses.Importing;

public sealed class LocalCourseFolderReader : ICourseFolderReader
{
    public CourseFolderSnapshot Read(ImportCourseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        string rootPath = Path.GetFullPath(command.RootPath);
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException("The selected course folder does not exist.");
        }

        List<CourseFileCandidate> videoFiles = [];
        List<RejectedCourseFile> rejectedFiles = [];

        foreach (FileInfo file in EnumerateFiles(new DirectoryInfo(rootPath), rootPath, rejectedFiles))
        {
            string relativePath = SafeRelativePath.From(rootPath, file.FullName);

            if (IsReparsePoint(file.Attributes))
            {
                rejectedFiles.Add(new RejectedCourseFile(relativePath, CourseFileRejectionReason.ReparsePointNotAllowed));
                continue;
            }

            if (AllowedCourseFileExtensions.IsVideo(file.FullName))
            {
                videoFiles.Add(new CourseFileCandidate(relativePath));
                continue;
            }

            rejectedFiles.Add(new RejectedCourseFile(relativePath, CourseFileRejectionReason.UnsupportedExtension));
        }

        return new CourseFolderSnapshot(
            Path.GetFileName(rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)),
            videoFiles,
            rejectedFiles);
    }

    private static IEnumerable<FileInfo> EnumerateFiles(
        DirectoryInfo directory,
        string rootPath,
        ICollection<RejectedCourseFile> rejectedFiles)
    {
        foreach (FileInfo file in directory.EnumerateFiles().OrderBy(file => file.FullName, StringComparer.OrdinalIgnoreCase))
        {
            yield return file;
        }

        foreach (DirectoryInfo childDirectory in directory.EnumerateDirectories().OrderBy(child => child.FullName, StringComparer.OrdinalIgnoreCase))
        {
            string relativePath = SafeRelativePath.From(rootPath, childDirectory.FullName);

            if (IsReparsePoint(childDirectory.Attributes))
            {
                rejectedFiles.Add(new RejectedCourseFile(relativePath, CourseFileRejectionReason.ReparsePointNotAllowed));
                continue;
            }

            foreach (FileInfo file in EnumerateFiles(childDirectory, rootPath, rejectedFiles))
            {
                yield return file;
            }
        }
    }

    private static bool IsReparsePoint(FileAttributes attributes)
    {
        return (attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
    }
}
