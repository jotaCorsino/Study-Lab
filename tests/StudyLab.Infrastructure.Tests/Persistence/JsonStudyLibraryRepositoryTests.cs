using System.Globalization;
using StudyLab.Application.Persistence;
using StudyLab.Infrastructure.Persistence;

namespace StudyLab.Infrastructure.Tests.Persistence;

public sealed class JsonStudyLibraryRepositoryTests
{
    [Fact]
    public void LoadReturnsEmptySnapshotWhenFileDoesNotExist()
    {
        using TemporaryStorage storage = TemporaryStorage.Create();
        JsonStudyLibraryRepository repository = new(storage.FilePath);

        StudyLibrarySnapshot snapshot = repository.Load();

        Assert.Empty(snapshot.Courses);
        Assert.Empty(snapshot.Progress);
    }

    [Fact]
    public void SaveThenLoadRoundTripsCatalogProgressAndPreferences()
    {
        using TemporaryStorage storage = TemporaryStorage.Create();
        JsonStudyLibraryRepository repository = new(storage.FilePath);
        StudyLibrarySnapshot snapshot = CreateSnapshot();

        repository.Save(snapshot);
        StudyLibrarySnapshot loadedSnapshot = repository.Load();

        CourseCatalogEntry course = Assert.Single(loadedSnapshot.Courses);
        Assert.Equal("Curso C#", course.Title);
        Assert.Equal("C:/courses/csharp", course.RootPath);
        Assert.Equal(CourseCatalogItemType.Folder, Assert.Single(course.Items).Type);
        Assert.True(Assert.Single(loadedSnapshot.Progress).IsCompleted);
        Assert.Equal(1.5m, loadedSnapshot.Preferences.DefaultPlaybackSpeed);
        Assert.True(loadedSnapshot.Preferences.IntroSkipEnabled);
        Assert.Equal(TimeSpan.FromSeconds(10), loadedSnapshot.Preferences.IntroSkipDuration);
    }

    [Fact]
    public void SaveUsesSingleJsonFileAndDoesNotLeaveTemporaryFiles()
    {
        using TemporaryStorage storage = TemporaryStorage.Create();
        JsonStudyLibraryRepository repository = new(storage.FilePath);

        repository.Save(CreateSnapshot());
        repository.Save(CreateSnapshot());

        string[] files = Directory.GetFiles(storage.RootPath);
        Assert.Equal([storage.FilePath], files);
    }

    [Fact]
    public void LoadRejectsInvalidJson()
    {
        using TemporaryStorage storage = TemporaryStorage.Create();
        Directory.CreateDirectory(storage.RootPath);
        File.WriteAllText(storage.FilePath, "{ invalid json");
        JsonStudyLibraryRepository repository = new(storage.FilePath);

        Assert.Throws<InvalidDataException>(repository.Load);
    }

    private static StudyLibrarySnapshot CreateSnapshot()
    {
        CourseCatalogItem lesson = new(
            CourseCatalogItemType.Lesson,
            "Aula 01",
            "Modulo/Aula 01.mp4",
            []);

        CourseCatalogItem module = new(
            CourseCatalogItemType.Folder,
            "Modulo",
            relativePath: null,
            [lesson]);

        CourseCatalogEntry course = new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "Curso C#",
            "C:/courses/csharp",
            [module],
            DateTimeOffset.Parse("2026-04-28T12:00:00Z", CultureInfo.InvariantCulture));

        LessonProgressEntry progress = new(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            TimeSpan.FromMinutes(12),
            isCompleted: true);

        StudyPreferences preferences = new(1.5m, introSkipEnabled: true, TimeSpan.FromSeconds(10));

        return new StudyLibrarySnapshot([course], [progress], preferences);
    }

    private sealed class TemporaryStorage : IDisposable
    {
        private TemporaryStorage(string rootPath)
        {
            RootPath = rootPath;
            FilePath = Path.Combine(rootPath, "library.json");
        }

        public string RootPath { get; }

        public string FilePath { get; }

        public static TemporaryStorage Create()
        {
            string rootPath = Path.Combine(Path.GetTempPath(), "StudyLab.Persistence.Tests", Guid.NewGuid().ToString("N"));
            return new TemporaryStorage(rootPath);
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
