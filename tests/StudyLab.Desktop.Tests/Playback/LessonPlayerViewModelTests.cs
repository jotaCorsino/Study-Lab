using System.Globalization;
using StudyLab.Application.Persistence;
using StudyLab.Application.Playback;
using StudyLab.Desktop.Presentation.Playback;

namespace StudyLab.Desktop.Tests.Playback;

public sealed class LessonPlayerViewModelTests
{
    [Fact]
    public void LoadProjectsPlayableLessonWithoutShowingLocalPathAsStatus()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        string rootPath = Path.Combine(Path.GetTempPath(), "StudyLab.Tests", "Curso CSharp");
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 01.mp4");
        LessonProgressEntry progress = new(lessonId, TimeSpan.FromMinutes(3), isCompleted: false);
        LessonPlayerViewModel viewModel = CreateViewModel(
            CreateSnapshot(CreateCourse(courseId, rootPath, "Modulo 1/Aula 01.mp4"), [progress]),
            courseId,
            lessonId);

        viewModel.Load();

        Assert.True(viewModel.IsLoaded);
        Assert.False(viewModel.HasError);
        Assert.Equal("Curso C#", viewModel.CourseTitle);
        Assert.Equal("Aula 01", viewModel.LessonTitle);
        Assert.Equal(Path.GetFullPath(Path.Combine(rootPath, "Modulo 1", "Aula 01.mp4")), viewModel.MediaPath);
        Assert.Equal("Pronto para reproduzir", viewModel.StatusMessage);
        Assert.Equal("3 min assistidos", viewModel.ProgressText);
        Assert.Equal(TimeSpan.FromMinutes(3), viewModel.ResumePosition);
        Assert.True(viewModel.ShouldResumePlayback);
        Assert.False(viewModel.IsCompleted);
        Assert.True(viewModel.CanMarkCompleted);
        Assert.DoesNotContain(rootPath, viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MarkCompletedPersistsProgressAndUpdatesState()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Aula 01.mp4");
        FakeStudyLibraryRepository repository = new(CreateSnapshot(
            CreateCourse(courseId, "C:/Courses/CSharp", "Modulo 1/Aula 01.mp4"),
            []));
        LessonPlayerViewModel viewModel = new(
            new LoadLessonPlaybackUseCase(repository),
            new RecordLessonProgressUseCase(repository),
            courseId,
            lessonId);

        viewModel.Load();
        viewModel.MarkCompleted(TimeSpan.FromMinutes(8));

        Assert.True(viewModel.IsCompleted);
        Assert.False(viewModel.CanMarkCompleted);
        Assert.Equal("Aula concluida", viewModel.StatusMessage);
        Assert.Equal("Concluida", viewModel.ProgressText);
        Assert.False(viewModel.ShouldResumePlayback);

        LessonProgressEntry savedProgress = Assert.Single(Assert.IsType<StudyLibrarySnapshot>(repository.SavedSnapshot).Progress);
        Assert.Equal(lessonId, savedProgress.LessonId);
        Assert.Equal(TimeSpan.FromMinutes(8), savedProgress.WatchedDuration);
        Assert.True(savedProgress.IsCompleted);
    }

    [Fact]
    public void LoadShowsSafeErrorWhenLessonDoesNotExist()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        LessonPlayerViewModel viewModel = CreateViewModel(
            CreateSnapshot(CreateCourse(courseId, "C:/Courses/CSharp", "Modulo 1/Aula 01.mp4"), []),
            courseId,
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        viewModel.Load();

        Assert.False(viewModel.IsLoaded);
        Assert.True(viewModel.HasError);
        Assert.Null(viewModel.MediaPath);
        Assert.Equal("Aula nao encontrada", viewModel.StatusMessage);
        Assert.False(viewModel.CanMarkCompleted);
        Assert.Equal(TimeSpan.Zero, viewModel.ResumePosition);
        Assert.False(viewModel.ShouldResumePlayback);
    }

    [Fact]
    public void LoadShowsSafeErrorWhenStoredMediaIsRejected()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        string rootPath = "C:/Courses/CSharp";
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Notas.txt");
        LessonPlayerViewModel viewModel = CreateViewModel(
            CreateSnapshot(CreateCourse(courseId, rootPath, "Modulo 1/Notas.txt"), []),
            courseId,
            lessonId);

        viewModel.Load();

        Assert.False(viewModel.IsLoaded);
        Assert.True(viewModel.HasError);
        Assert.Null(viewModel.MediaPath);
        Assert.Equal("Nao foi possivel abrir esta aula com seguranca", viewModel.StatusMessage);
        Assert.False(viewModel.CanMarkCompleted);
        Assert.Equal(TimeSpan.Zero, viewModel.ResumePosition);
        Assert.False(viewModel.ShouldResumePlayback);
        Assert.DoesNotContain(rootPath, viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    private static LessonPlayerViewModel CreateViewModel(StudyLibrarySnapshot snapshot, Guid courseId, Guid lessonId)
    {
        FakeStudyLibraryRepository repository = new(snapshot);

        return new LessonPlayerViewModel(
            new LoadLessonPlaybackUseCase(repository),
            new RecordLessonProgressUseCase(repository),
            courseId,
            lessonId);
    }

    private static StudyLibrarySnapshot CreateSnapshot(CourseCatalogEntry course, IEnumerable<LessonProgressEntry> progress)
    {
        return new StudyLibrarySnapshot([course], progress, StudyPreferences.Default);
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
