using System.Globalization;
using StudyLab.Application.Courses.Importing;
using StudyLab.Application.Persistence;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop.Tests.Catalog;

public sealed class CatalogViewModelTests
{
    [Fact]
    public void LoadShowsEmptyCatalogMessageWhenLibraryHasNoCourses()
    {
        CatalogViewModel viewModel = CreateViewModel(new FakeStudyLibraryRepository());

        viewModel.Load();

        Assert.False(viewModel.HasCourses);
        Assert.Equal("Nenhum curso importado", viewModel.CourseCountText);
        Assert.Empty(viewModel.Courses);
    }

    [Fact]
    public void LoadProjectsCoursesIntoCatalogItems()
    {
        CourseCatalogEntry course = CreateCourse("Curso C#", lessonCount: 3);
        CatalogViewModel viewModel = CreateViewModel(new FakeStudyLibraryRepository(
            new StudyLibrarySnapshot([course], [], StudyPreferences.Default)));

        viewModel.Load();

        CatalogCourseViewModel item = Assert.Single(viewModel.Courses);
        Assert.Equal(course.Id, item.Id);
        Assert.Equal("Curso C#", item.Title);
        Assert.Equal(3, item.LessonCount);
        Assert.Equal("3 aulas", item.LessonCountText);
        Assert.True(viewModel.HasCourses);
        Assert.Equal("1 curso no catalogo", viewModel.CourseCountText);
    }

    [Fact]
    public void CourseCatalogItemDoesNotExposeLocalRootPath()
    {
        string[] publicPropertyNames = typeof(CatalogCourseViewModel)
            .GetProperties()
            .Select(property => property.Name)
            .ToArray();

        Assert.DoesNotContain(publicPropertyNames, property => property.Contains("Path", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(publicPropertyNames, property => property.Contains("Root", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ImportCourseAsyncDoesNothingWhenFolderSelectionIsCanceled()
    {
        FakeStudyLibraryRepository repository = new();
        CatalogViewModel viewModel = CreateViewModel(repository, picker: new FakeCourseFolderPicker(null));

        await viewModel.ImportCourseAsync();

        Assert.Null(repository.SavedSnapshot);
        Assert.Equal("Importacao cancelada", viewModel.StatusMessage);
        Assert.False(viewModel.IsImporting);
    }

    [Fact]
    public async Task ImportCourseAsyncImportsSelectedFolderAndRefreshesCatalog()
    {
        FakeStudyLibraryRepository repository = new();
        CatalogViewModel viewModel = CreateViewModel(
            repository,
            new FakeCourseFolderReader(new CourseFolderSnapshot(
                "Curso C#",
                [new CourseFileCandidate("Modulo/Aula 01.mp4")],
                [])),
            new FakeCourseFolderPicker("D:/Courses/CSharp"));

        await viewModel.ImportCourseAsync();

        Assert.NotNull(repository.SavedSnapshot);
        CatalogCourseViewModel course = Assert.Single(viewModel.Courses);
        Assert.Equal("Curso C#", course.Title);
        Assert.Equal("Curso importado com sucesso", viewModel.StatusMessage);
        Assert.False(viewModel.IsImporting);
    }

    [Fact]
    public async Task ImportCourseAsyncShowsRejectedFilesWithSafeRelativeLocations()
    {
        FakeStudyLibraryRepository repository = new();
        CatalogViewModel viewModel = CreateViewModel(
            repository,
            new FakeCourseFolderReader(new CourseFolderSnapshot(
                "Curso C#",
                [new CourseFileCandidate("Modulo/Aula 01.mp4")],
                [
                    new RejectedCourseFile("Modulo/Anotacoes.txt", CourseFileRejectionReason.UnsupportedExtension),
                    new RejectedCourseFile("Modulo/Atalho.lnk", CourseFileRejectionReason.ReparsePointNotAllowed)
                ])),
            new FakeCourseFolderPicker("D:/Courses/CSharp"));

        await viewModel.ImportCourseAsync();

        Assert.True(viewModel.HasRejectedFiles);
        Assert.Equal("2 arquivos ignorados", viewModel.RejectedFilesSummary);
        Assert.Equal("Curso importado com 2 arquivos ignorados", viewModel.StatusMessage);

        RejectedCourseFileViewModel firstRejectedFile = viewModel.RejectedFiles[0];
        Assert.Equal("Modulo/Anotacoes.txt", firstRejectedFile.Location);
        Assert.Equal("Extensao nao suportada", firstRejectedFile.ReasonText);
        Assert.DoesNotContain("D:/Courses/CSharp", firstRejectedFile.Location, StringComparison.OrdinalIgnoreCase);

        RejectedCourseFileViewModel secondRejectedFile = viewModel.RejectedFiles[1];
        Assert.Equal("Modulo/Atalho.lnk", secondRejectedFile.Location);
        Assert.Equal("Atalho ou link nao permitido", secondRejectedFile.ReasonText);
    }

    [Fact]
    public async Task ImportCourseAsyncUsesSafeErrorMessageWhenImportFails()
    {
        FakeStudyLibraryRepository repository = new();
        CatalogViewModel viewModel = CreateViewModel(
            repository,
            new ThrowingCourseFolderReader(new UnauthorizedAccessException("Access denied to D:/Courses/Private")),
            new FakeCourseFolderPicker("D:/Courses/Private"));

        await viewModel.ImportCourseAsync();

        Assert.Equal("Nao foi possivel importar o curso selecionado.", viewModel.StatusMessage);
        Assert.DoesNotContain("D:/Courses/Private", viewModel.StatusMessage, StringComparison.OrdinalIgnoreCase);
        Assert.False(viewModel.HasRejectedFiles);
        Assert.Empty(viewModel.RejectedFiles);
        Assert.False(viewModel.IsImporting);
    }

    [Fact]
    public void RejectedCourseFileViewModelDoesNotExposeLocalRootPath()
    {
        string[] publicPropertyNames = typeof(RejectedCourseFileViewModel)
            .GetProperties()
            .Select(property => property.Name)
            .ToArray();

        Assert.DoesNotContain(publicPropertyNames, property => property.Contains("Root", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(publicPropertyNames, property => property.Contains("Absolute", StringComparison.OrdinalIgnoreCase));
    }

    private static CourseCatalogEntry CreateCourse(string title, int lessonCount)
    {
        CourseCatalogItem[] lessons = Enumerable.Range(1, lessonCount)
            .Select(index => new CourseCatalogItem(
                CourseCatalogItemType.Lesson,
                FormattableString.Invariant($"Aula {index:00}"),
                FormattableString.Invariant($"Modulo/Aula {index:00}.mp4"),
                []))
            .ToArray();

        CourseCatalogItem module = new(
            CourseCatalogItemType.Folder,
            "Modulo 1",
            null,
            lessons);

        return new CourseCatalogEntry(
            Guid.NewGuid(),
            title,
            "D:/Courses/CSharp",
            [module],
            DateTimeOffset.Parse("2026-04-28T12:00:00Z", CultureInfo.InvariantCulture));
    }

    private static CatalogViewModel CreateViewModel(
        FakeStudyLibraryRepository repository,
        ICourseFolderReader? reader = null,
        ICourseFolderPicker? picker = null)
    {
        ICourseFolderReader courseFolderReader = reader ?? new FakeCourseFolderReader(new CourseFolderSnapshot("Curso C#", [], []));

        return new CatalogViewModel(
            new LoadStudyLibraryUseCase(repository),
            new ImportCourseToLibraryUseCase(new ImportCourseFromFolderUseCase(courseFolderReader), repository),
            picker ?? new FakeCourseFolderPicker(null),
            () => Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            () => DateTimeOffset.Parse("2026-05-04T10:00:00Z", CultureInfo.InvariantCulture));
    }

    private sealed class FakeCourseFolderPicker(string? selectedPath) : ICourseFolderPicker
    {
        public Task<string?> PickFolderAsync()
        {
            return Task.FromResult(selectedPath);
        }
    }

    private sealed class FakeCourseFolderReader(CourseFolderSnapshot snapshot) : ICourseFolderReader
    {
        public CourseFolderSnapshot Read(ImportCourseCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            return snapshot;
        }
    }

    private sealed class ThrowingCourseFolderReader(Exception exception) : ICourseFolderReader
    {
        public CourseFolderSnapshot Read(ImportCourseCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            throw exception;
        }
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

        public StudyLibrarySnapshot Load()
        {
            return SavedSnapshot ?? _snapshot;
        }

        public void Save(StudyLibrarySnapshot snapshot)
        {
            SavedSnapshot = snapshot;
        }

        public StudyLibrarySnapshot? SavedSnapshot { get; private set; }
    }
}
