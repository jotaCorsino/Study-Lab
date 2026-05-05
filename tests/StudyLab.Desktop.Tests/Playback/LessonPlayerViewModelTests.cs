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
        LessonPlayerViewModel viewModel = CreateViewModel(
            CreateCourse(courseId, rootPath, "Modulo 1/Aula 01.mp4"),
            courseId,
            lessonId);

        viewModel.Load();

        Assert.True(viewModel.IsLoaded);
        Assert.False(viewModel.HasError);
        Assert.Equal("Curso C#", viewModel.CourseTitle);
        Assert.Equal("Aula 01", viewModel.LessonTitle);
        Assert.Equal(Path.GetFullPath(Path.Combine(rootPath, "Modulo 1", "Aula 01.mp4")), viewModel.MediaPath);
        Assert.Equal("Pronto para reproduzir", viewModel.StatusMessage);
        Assert.DoesNotContain(rootPath, viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LoadShowsSafeErrorWhenLessonDoesNotExist()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        LessonPlayerViewModel viewModel = CreateViewModel(
            CreateCourse(courseId, "C:/Courses/CSharp", "Modulo 1/Aula 01.mp4"),
            courseId,
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        viewModel.Load();

        Assert.False(viewModel.IsLoaded);
        Assert.True(viewModel.HasError);
        Assert.Null(viewModel.MediaPath);
        Assert.Equal("Aula nao encontrada", viewModel.StatusMessage);
    }

    [Fact]
    public void LoadShowsSafeErrorWhenStoredMediaIsRejected()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        string rootPath = "C:/Courses/CSharp";
        Guid lessonId = LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, "Modulo 1/Notas.txt");
        LessonPlayerViewModel viewModel = CreateViewModel(
            CreateCourse(courseId, rootPath, "Modulo 1/Notas.txt"),
            courseId,
            lessonId);

        viewModel.Load();

        Assert.False(viewModel.IsLoaded);
        Assert.True(viewModel.HasError);
        Assert.Null(viewModel.MediaPath);
        Assert.Equal("Nao foi possivel abrir esta aula com seguranca", viewModel.StatusMessage);
        Assert.DoesNotContain(rootPath, viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
    }

    private static LessonPlayerViewModel CreateViewModel(CourseCatalogEntry course, Guid courseId, Guid lessonId)
    {
        StudyLibrarySnapshot snapshot = new([course], [], StudyPreferences.Default);

        return new LessonPlayerViewModel(
            new LoadLessonPlaybackUseCase(new FakeStudyLibraryRepository(snapshot)),
            courseId,
            lessonId);
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
            throw new NotSupportedException("Lesson player view model does not save data.");
        }
    }
}
