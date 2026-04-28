using StudyLab.Application.Courses.Importing;
using StudyLab.Infrastructure.Courses.Importing;

namespace StudyLab.Infrastructure.Tests.Courses;

public sealed class LocalCourseFolderReaderTests
{
    [Fact]
    public void ReadReturnsAllowedVideosAndRejectsUnsupportedFiles()
    {
        using TemporaryCourseFolder folder = TemporaryCourseFolder.Create();
        folder.WriteFile("Modulo/Topico/Aula 01.mp4");
        folder.WriteFile("Modulo/Topico/Aula 02.mkv");
        folder.WriteFile("Modulo/Topico/Notas.txt");

        LocalCourseFolderReader reader = new();

        CourseFolderSnapshot snapshot = reader.Read(new ImportCourseCommand(folder.RootPath));

        Assert.Equal(folder.CourseTitle, snapshot.CourseTitle);
        Assert.Collection(
            snapshot.VideoFiles,
            file => Assert.Equal("Modulo/Topico/Aula 01.mp4", file.RelativePath),
            file => Assert.Equal("Modulo/Topico/Aula 02.mkv", file.RelativePath));
        RejectedCourseFile rejectedFile = Assert.Single(snapshot.RejectedFiles);
        Assert.Equal("Modulo/Topico/Notas.txt", rejectedFile.RelativePath);
        Assert.Equal(CourseFileRejectionReason.UnsupportedExtension, rejectedFile.Reason);
    }

    [Fact]
    public void SafeRelativePathRejectsPathsOutsideRoot()
    {
        string root = Path.Combine(Path.GetTempPath(), "StudyLab", "root");
        string outside = Path.Combine(Path.GetTempPath(), "StudyLab", "outside.mp4");

        Assert.Throws<UnauthorizedAccessException>(() =>
            SafeRelativePath.From(root, outside));
    }

    [Fact]
    public void ReadRejectsMissingRootDirectory()
    {
        LocalCourseFolderReader reader = new();
        string missingRoot = Path.Combine(Path.GetTempPath(), "StudyLab", Guid.NewGuid().ToString("N"));

        Assert.Throws<DirectoryNotFoundException>(() =>
            reader.Read(new ImportCourseCommand(missingRoot)));
    }

    private sealed class TemporaryCourseFolder : IDisposable
    {
        private TemporaryCourseFolder(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public string CourseTitle => Path.GetFileName(RootPath);

        public static TemporaryCourseFolder Create()
        {
            string rootPath = Path.Combine(Path.GetTempPath(), "StudyLab.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(rootPath);
            return new TemporaryCourseFolder(rootPath);
        }

        public void WriteFile(string relativePath)
        {
            string filePath = Path.Combine(RootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, "test");
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
