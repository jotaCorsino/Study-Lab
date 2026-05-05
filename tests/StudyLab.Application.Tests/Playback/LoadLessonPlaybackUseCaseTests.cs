using System.Globalization;
using StudyLab.Application.Persistence;
using StudyLab.Application.Playback;

namespace StudyLab.Application.Tests.Playback;

public sealed class LoadLessonPlaybackUseCaseTests
{
    [Fact]
    public void LoadReturnsPlayableLessonInsideStoredCourseRoot()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        string rootPath = Path.Combine(Path.GetTempPath(), "StudyLab.Tests", "Curso CSharp");
        CourseCatalogEntry course = CreateCourse(courseId, rootPath, "Modulo 1/Aula 01.mp4");
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 01.mp4");
        LoadLessonPlaybackUseCase useCase = new(new FakeStudyLibraryRepository(new StudyLibrarySnapshot(
            [course],
            [],
            StudyPreferences.Default)));

        LessonPlayback playback = Assert.IsType<LessonPlayback>(useCase.Load(new LoadLessonPlaybackCommand(courseId, lessonId)));

        Assert.Equal(courseId, playback.CourseId);
        Assert.Equal(lessonId, playback.LessonId);
        Assert.Equal("Curso C#", playback.CourseTitle);
        Assert.Equal("Aula 01", playback.LessonTitle);
        Assert.Equal(Path.GetFullPath(Path.Combine(rootPath, "Modulo 1", "Aula 01.mp4")), playback.MediaPath);
    }

    [Fact]
    public void LoadReturnsNullWhenLessonDoesNotExist()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseCatalogEntry course = CreateCourse(courseId, "C:/Courses/CSharp", "Modulo 1/Aula 01.mp4");
        LoadLessonPlaybackUseCase useCase = new(new FakeStudyLibraryRepository(new StudyLibrarySnapshot(
            [course],
            [],
            StudyPreferences.Default)));

        LessonPlayback? playback = useCase.Load(new LoadLessonPlaybackCommand(
            courseId,
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")));

        Assert.Null(playback);
    }

    [Fact]
    public void LoadRejectsUnsupportedStoredMediaExtension()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseCatalogEntry course = CreateCourse(courseId, "C:/Courses/CSharp", "Modulo 1/Notas.txt");
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Notas.txt");
        LoadLessonPlaybackUseCase useCase = new(new FakeStudyLibraryRepository(new StudyLibrarySnapshot(
            [course],
            [],
            StudyPreferences.Default)));

        Assert.Throws<InvalidDataException>(() =>
            useCase.Load(new LoadLessonPlaybackCommand(courseId, lessonId)));
    }

    [Fact]
    public void CommandRejectsEmptyIds()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        Guid lessonId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        Assert.Throws<ArgumentException>(() => new LoadLessonPlaybackCommand(Guid.Empty, lessonId));
        Assert.Throws<ArgumentException>(() => new LoadLessonPlaybackCommand(courseId, Guid.Empty));
    }

    [Fact]
    public void LessonIdentityIsStableForCourseAndRelativePath()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        Guid first = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 01.mp4");
        Guid second = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1\\Aula 01.mp4");
        Guid otherLesson = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 02.mp4");

        Assert.NotEqual(Guid.Empty, first);
        Assert.Equal(first, second);
        Assert.NotEqual(first, otherLesson);
    }

    private static CourseCatalogEntry CreateCourse(Guid courseId, string rootPath, string lessonRelativePath)
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
            rootPath,
            [module],
            DateTimeOffset.Parse("2026-05-05T10:00:00Z", CultureInfo.InvariantCulture));
    }

    private sealed class FakeStudyLibraryRepository(StudyLibrarySnapshot snapshot) : IStudyLibraryRepository
    {
        public StudyLibrarySnapshot Load()
        {
            return snapshot;
        }

        public void Save(StudyLibrarySnapshot snapshotToSave)
        {
            throw new NotSupportedException("Load lesson playback does not save data.");
        }
    }
}
