using System.Globalization;
using StudyLab.Application.Persistence;

namespace StudyLab.Application.Tests.Persistence;

public sealed class StudyLibraryUseCaseTests
{
    [Fact]
    public void SavePassesSnapshotToRepository()
    {
        FakeStudyLibraryRepository repository = new();
        SaveStudyLibraryUseCase useCase = new(repository);
        StudyLibrarySnapshot snapshot = CreateSnapshot();

        useCase.Save(snapshot);

        Assert.Same(snapshot, repository.SavedSnapshot);
    }

    [Fact]
    public void LoadReturnsSnapshotFromRepository()
    {
        StudyLibrarySnapshot snapshot = CreateSnapshot();
        FakeStudyLibraryRepository repository = new(snapshot);
        LoadStudyLibraryUseCase useCase = new(repository);

        StudyLibrarySnapshot loadedSnapshot = useCase.Load();

        Assert.Same(snapshot, loadedSnapshot);
    }

    [Fact]
    public void EmptySnapshotHasSafeDefaults()
    {
        StudyLibrarySnapshot snapshot = StudyLibrarySnapshot.Empty;

        Assert.Empty(snapshot.Courses);
        Assert.Empty(snapshot.Progress);
        Assert.Equal(1.0m, snapshot.Preferences.DefaultPlaybackSpeed);
        Assert.False(snapshot.Preferences.IntroSkipEnabled);
        Assert.Equal(TimeSpan.Zero, snapshot.Preferences.IntroSkipDuration);
    }

    [Fact]
    public void CourseCatalogEntryRejectsEmptyTitle()
    {
        Assert.Throws<ArgumentException>(() =>
            new CourseCatalogEntry(Guid.NewGuid(), " ", "C:/courses/csharp", [], DateTimeOffset.UtcNow));
    }

    private static StudyLibrarySnapshot CreateSnapshot()
    {
        CourseCatalogItem lesson = new(
            CourseCatalogItemType.Lesson,
            "Aula 01",
            "Modulo/Aula 01.mp4",
            []);

        CourseCatalogEntry course = new(
            Guid.NewGuid(),
            "Curso C#",
            "C:/courses/csharp",
            [lesson],
            DateTimeOffset.Parse("2026-04-28T12:00:00Z", CultureInfo.InvariantCulture));

        LessonProgressEntry progress = new(
            Guid.NewGuid(),
            TimeSpan.FromMinutes(12),
            isCompleted: true);

        StudyPreferences preferences = new(1.5m, introSkipEnabled: true, TimeSpan.FromSeconds(10));

        return new StudyLibrarySnapshot([course], [progress], preferences);
    }

    private sealed class FakeStudyLibraryRepository : IStudyLibraryRepository
    {
        private readonly StudyLibrarySnapshot _snapshot;

        public FakeStudyLibraryRepository()
            : this(StudyLibrarySnapshot.Empty)
        {
        }

        public FakeStudyLibraryRepository(StudyLibrarySnapshot snapshot)
        {
            _snapshot = snapshot;
        }

        public StudyLibrarySnapshot? SavedSnapshot { get; private set; }

        public StudyLibrarySnapshot Load()
        {
            return _snapshot;
        }

        public void Save(StudyLibrarySnapshot snapshot)
        {
            SavedSnapshot = snapshot;
        }
    }
}
