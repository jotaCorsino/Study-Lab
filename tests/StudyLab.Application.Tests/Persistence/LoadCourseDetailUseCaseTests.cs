using System.Globalization;
using StudyLab.Application.Persistence;

namespace StudyLab.Application.Tests.Persistence;

public sealed class LoadCourseDetailUseCaseTests
{
    [Fact]
    public void LoadReturnsCourseDetailWithoutLocalRootPath()
    {
        Guid courseId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        CourseCatalogEntry course = CreateCourse(courseId);
        LoadCourseDetailUseCase useCase = new(new FakeStudyLibraryRepository(new StudyLibrarySnapshot(
            [course],
            [],
            StudyPreferences.Default)));

        CourseDetail detail = Assert.IsType<CourseDetail>(useCase.Load(courseId));

        Assert.Equal(courseId, detail.Id);
        Assert.Equal("Curso C#", detail.Title);
        Assert.Equal(DateTimeOffset.Parse("2026-05-04T10:00:00Z", CultureInfo.InvariantCulture), detail.ImportedAt);
        Assert.Equal(2, detail.LessonCount);

        string[] detailProperties = typeof(CourseDetail).GetProperties().Select(property => property.Name).ToArray();
        Assert.DoesNotContain(detailProperties, property => property.Contains("Root", StringComparison.OrdinalIgnoreCase));

        CourseDetailItem module = Assert.Single(detail.Items);
        Assert.Equal(CourseCatalogItemType.Folder, module.Type);
        Assert.Equal("Modulo 1", module.Title);

        Assert.Equal(2, module.Children.Count);
        CourseDetailItem lesson = module.Children[0];
        Assert.Equal(CourseCatalogItemType.Lesson, lesson.Type);
        Assert.Equal("Aula 01", lesson.Title);
        Assert.Equal("Modulo 1/Aula 01.mp4", lesson.RelativePath);

        CourseDetailItem topic = module.Children[1];
        Assert.Equal(CourseCatalogItemType.Folder, topic.Type);
        Assert.Equal("Topico 1", topic.Title);
    }

    [Fact]
    public void LoadReturnsNullWhenCourseDoesNotExist()
    {
        LoadCourseDetailUseCase useCase = new(new FakeStudyLibraryRepository(StudyLibrarySnapshot.Empty));

        CourseDetail? detail = useCase.Load(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

        Assert.Null(detail);
    }

    [Fact]
    public void LoadRejectsEmptyCourseId()
    {
        LoadCourseDetailUseCase useCase = new(new FakeStudyLibraryRepository(StudyLibrarySnapshot.Empty));

        Assert.Throws<ArgumentException>(() => useCase.Load(Guid.Empty));
    }

    private static CourseCatalogEntry CreateCourse(Guid courseId)
    {
        CourseCatalogItem topic = new(
            CourseCatalogItemType.Folder,
            "Topico 1",
            null,
            [
                new CourseCatalogItem(
                    CourseCatalogItemType.Lesson,
                    "Aula 02",
                    "Modulo 1/Topico 1/Aula 02.mp4",
                    [])
            ]);

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
                topic
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
            throw new NotSupportedException("Load course detail does not save data.");
        }
    }
}
