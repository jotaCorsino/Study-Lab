using System.Globalization;
using StudyLab.Application.Persistence;
using StudyLab.Application.Playback;

namespace StudyLab.Application.Tests.Playback;

public sealed class RecordLessonProgressUseCaseTests
{
    [Fact]
    public void RecordAddsProgressForExistingLessonAndPreservesSnapshotData()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseCatalogEntry course = CreateCourse(courseId, "Modulo 1/Aula 01.mp4");
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 01.mp4");
        StudyPreferences preferences = new(1.25m, introSkipEnabled: true, TimeSpan.FromSeconds(15));
        FakeStudyLibraryRepository repository = new(new StudyLibrarySnapshot([course], [], preferences));
        RecordLessonProgressUseCase useCase = new(repository);

        LessonProgressEntry progress = Assert.IsType<LessonProgressEntry>(useCase.Record(new RecordLessonProgressCommand(
            courseId,
            lessonId,
            TimeSpan.FromMinutes(12),
            isCompleted: true)));

        Assert.Equal(lessonId, progress.LessonId);
        Assert.Equal(TimeSpan.FromMinutes(12), progress.WatchedDuration);
        Assert.True(progress.IsCompleted);

        StudyLibrarySnapshot savedSnapshot = Assert.IsType<StudyLibrarySnapshot>(repository.SavedSnapshot);
        Assert.Same(course, Assert.Single(savedSnapshot.Courses));
        Assert.Same(preferences, savedSnapshot.Preferences);
        LessonProgressEntry savedProgress = Assert.Single(savedSnapshot.Progress);
        Assert.Equal(lessonId, savedProgress.LessonId);
        Assert.True(savedProgress.IsCompleted);
    }

    [Fact]
    public void RecordReplacesExistingProgressWithoutDecreasingWatchedDuration()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseCatalogEntry course = CreateCourse(courseId, "Modulo 1/Aula 01.mp4");
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 01.mp4");
        LessonProgressEntry existingProgress = new(lessonId, TimeSpan.FromMinutes(20), isCompleted: false);
        FakeStudyLibraryRepository repository = new(new StudyLibrarySnapshot(
            [course],
            [existingProgress],
            StudyPreferences.Default));
        RecordLessonProgressUseCase useCase = new(repository);

        LessonProgressEntry progress = Assert.IsType<LessonProgressEntry>(useCase.Record(new RecordLessonProgressCommand(
            courseId,
            lessonId,
            TimeSpan.FromMinutes(5),
            isCompleted: true)));

        Assert.Equal(TimeSpan.FromMinutes(20), progress.WatchedDuration);
        Assert.True(progress.IsCompleted);
        Assert.Single(Assert.IsType<StudyLibrarySnapshot>(repository.SavedSnapshot).Progress);
    }

    [Fact]
    public void RecordReturnsNullAndDoesNotSaveWhenLessonDoesNotExist()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseCatalogEntry course = CreateCourse(courseId, "Modulo 1/Aula 01.mp4");
        FakeStudyLibraryRepository repository = new(new StudyLibrarySnapshot(
            [course],
            [],
            StudyPreferences.Default));
        RecordLessonProgressUseCase useCase = new(repository);

        LessonProgressEntry? progress = useCase.Record(new RecordLessonProgressCommand(
            courseId,
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            TimeSpan.FromMinutes(1),
            isCompleted: true));

        Assert.Null(progress);
        Assert.Null(repository.SavedSnapshot);
    }

    [Fact]
    public void CommandRejectsInvalidValues()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        Guid lessonId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        Assert.Throws<ArgumentException>(() => new RecordLessonProgressCommand(Guid.Empty, lessonId, TimeSpan.Zero, false));
        Assert.Throws<ArgumentException>(() => new RecordLessonProgressCommand(courseId, Guid.Empty, TimeSpan.Zero, false));
        Assert.Throws<ArgumentOutOfRangeException>(() => new RecordLessonProgressCommand(courseId, lessonId, TimeSpan.FromSeconds(-1), false));
    }

    private static CourseCatalogEntry CreateCourse(Guid courseId, string lessonRelativePath)
    {
        CourseCatalogItem module = new(
            CourseCatalogItemType.Folder,
            "Modulo 1",
            null,
            [
                new CourseCatalogItem(
                    CourseCatalogItemType.Lesson,
                    "Aula 01",
                    lessonRelativePath,
                    [])
            ]);

        return new CourseCatalogEntry(
            courseId,
            "Curso C#",
            "C:/Courses/CSharp",
            [module],
            DateTimeOffset.Parse("2026-05-05T10:00:00Z", CultureInfo.InvariantCulture));
    }

    private sealed class FakeStudyLibraryRepository(StudyLibrarySnapshot snapshot) : IStudyLibraryRepository
    {
        public StudyLibrarySnapshot? SavedSnapshot { get; private set; }

        public StudyLibrarySnapshot Load()
        {
            return snapshot;
        }

        public void Save(StudyLibrarySnapshot snapshotToSave)
        {
            SavedSnapshot = snapshotToSave;
        }
    }
}
