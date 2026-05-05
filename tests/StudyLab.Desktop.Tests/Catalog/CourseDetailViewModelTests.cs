using System.Globalization;
using StudyLab.Application.Persistence;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop.Tests.Catalog;

public sealed class CourseDetailViewModelTests
{
    [Fact]
    public void LoadProjectsCourseStructureForDisplay()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseDetailViewModel viewModel = CreateViewModel(CreateCourse(courseId), courseId);

        viewModel.Load();

        Assert.True(viewModel.IsFound);
        Assert.Equal("Curso C#", viewModel.Title);
        Assert.Equal("2 aulas", viewModel.LessonCountText);
        string expectedImportedAtText = DateTimeOffset
            .Parse("2026-05-04T10:00:00Z", CultureInfo.InvariantCulture)
            .ToLocalTime()
            .ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        Assert.Equal($"Importado em {expectedImportedAtText}", viewModel.ImportedAtText);
        Assert.True(viewModel.HasItems);

        CourseDetailItemViewModel module = Assert.Single(viewModel.Items);
        Assert.Equal("Modulo 1", module.Title);
        Assert.Equal("Modulo", module.KindText);
        Assert.Equal(2, module.Children.Count);

        CourseDetailItemViewModel firstLesson = module.Children[0];
        Assert.Equal("Aula 01", firstLesson.Title);
        Assert.Equal("Aula", firstLesson.KindText);
        Assert.True(firstLesson.CanOpenLesson);
        Assert.NotNull(firstLesson.LessonId);
        Assert.False(module.CanOpenLesson);
        Assert.Null(module.LessonId);
    }

    [Fact]
    public void LoadShowsSafeNotFoundStateWhenCourseDoesNotExist()
    {
        CourseDetailViewModel viewModel = CreateViewModel(null, Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        viewModel.Load();

        Assert.False(viewModel.IsFound);
        Assert.Equal("Curso nao encontrado", viewModel.Title);
        Assert.Equal("Nenhuma estrutura disponivel", viewModel.LessonCountText);
        Assert.False(viewModel.HasItems);
        Assert.Empty(viewModel.Items);
    }

    [Fact]
    public void CourseDetailViewModelsDoNotExposeLocalPaths()
    {
        string[] detailProperties = typeof(CourseDetailViewModel).GetProperties()
            .Select(property => property.Name)
            .ToArray();
        string[] itemProperties = typeof(CourseDetailItemViewModel).GetProperties()
            .Select(property => property.Name)
            .ToArray();

        Assert.DoesNotContain(detailProperties, property => property.Contains("Path", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(detailProperties, property => property.Contains("Root", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(itemProperties, property => property.Contains("Path", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(itemProperties, property => property.Contains("Root", StringComparison.OrdinalIgnoreCase));
    }

    private static CourseDetailViewModel CreateViewModel(CourseCatalogEntry? course, Guid courseId)
    {
        StudyLibrarySnapshot snapshot = course is null
            ? StudyLibrarySnapshot.Empty
            : new StudyLibrarySnapshot([course], [], StudyPreferences.Default);

        return new CourseDetailViewModel(
            new LoadCourseDetailUseCase(new FakeStudyLibraryRepository(snapshot)),
            courseId);
    }

    private static CourseCatalogEntry CreateCourse(Guid courseId)
    {
        CourseCatalogItem module = new(
            CourseCatalogItemType.Folder,
            "Modulo 1",
            null,
            [
                new CourseCatalogItem(
                    CourseCatalogItemType.Lesson,
                    "Aula 01",
                    "Modulo 1/Aula 01.mp4",
                    []),
                new CourseCatalogItem(
                    CourseCatalogItemType.Lesson,
                    "Aula 02",
                    "Modulo 1/Aula 02.mp4",
                    [])
            ]);

        return new CourseCatalogEntry(
            courseId,
            "Curso C#",
            "D:/Courses/CSharp",
            [module],
            DateTimeOffset.Parse("2026-05-04T10:00:00Z", CultureInfo.InvariantCulture));
    }

    private sealed class FakeStudyLibraryRepository(StudyLibrarySnapshot snapshot) : IStudyLibraryRepository
    {
        public StudyLibrarySnapshot Load()
        {
            return snapshot;
        }

        public void Save(StudyLibrarySnapshot snapshotToSave)
        {
            throw new NotSupportedException("Course detail view model does not save data.");
        }
    }
}
