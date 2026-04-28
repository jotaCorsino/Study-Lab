using System.Globalization;
using StudyLab.Application.Persistence;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop.Tests.Catalog;

public sealed class CatalogViewModelTests
{
    [Fact]
    public void LoadShowsEmptyCatalogMessageWhenLibraryHasNoCourses()
    {
        CatalogViewModel viewModel = new(new LoadStudyLibraryUseCase(new FakeStudyLibraryRepository()));

        viewModel.Load();

        Assert.False(viewModel.HasCourses);
        Assert.Equal("Nenhum curso importado", viewModel.CourseCountText);
        Assert.Empty(viewModel.Courses);
    }

    [Fact]
    public void LoadProjectsCoursesIntoCatalogItems()
    {
        CourseCatalogEntry course = CreateCourse("Curso C#", lessonCount: 3);
        CatalogViewModel viewModel = new(new LoadStudyLibraryUseCase(new FakeStudyLibraryRepository(
            new StudyLibrarySnapshot([course], [], StudyPreferences.Default))));

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
            return _snapshot;
        }

        public void Save(StudyLibrarySnapshot snapshot)
        {
            throw new NotSupportedException();
        }
    }
}
