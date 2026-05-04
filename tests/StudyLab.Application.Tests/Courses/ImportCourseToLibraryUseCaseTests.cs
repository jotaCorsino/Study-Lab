using System.Globalization;
using StudyLab.Application.Courses.Importing;
using StudyLab.Application.Persistence;

namespace StudyLab.Application.Tests.Courses;

public sealed class ImportCourseToLibraryUseCaseTests
{
    [Fact]
    public void ImportAddsCourseToLibraryAndPreservesExistingSnapshotData()
    {
        CourseCatalogEntry existingCourse = CreateExistingCourse();
        LessonProgressEntry progress = new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), TimeSpan.FromMinutes(10), isCompleted: true);
        StudyPreferences preferences = new(1.25m, introSkipEnabled: true, TimeSpan.FromSeconds(15));
        FakeStudyLibraryRepository repository = new(new StudyLibrarySnapshot([existingCourse], [progress], preferences));
        RejectedCourseFile rejectedFile = new("Anotacoes.txt", CourseFileRejectionReason.UnsupportedExtension);
        FakeCourseFolderReader reader = new(new CourseFolderSnapshot(
            "Curso C#",
            [new CourseFileCandidate("Modulo/Aula 01.mp4")],
            [rejectedFile]));
        ImportCourseToLibraryUseCase useCase = new(new ImportCourseFromFolderUseCase(reader), repository);
        Guid courseId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        DateTimeOffset importedAt = DateTimeOffset.Parse("2026-05-04T10:00:00Z", CultureInfo.InvariantCulture);

        CourseLibraryImportResult result = useCase.Import(new ImportCourseToLibraryCommand(
            "D:/Courses/CSharp",
            courseId,
            importedAt));

        StudyLibrarySnapshot savedSnapshot = Assert.IsType<StudyLibrarySnapshot>(repository.SavedSnapshot);
        Assert.Equal(2, savedSnapshot.Courses.Count);
        Assert.Same(existingCourse, savedSnapshot.Courses[0]);
        Assert.Same(progress, Assert.Single(savedSnapshot.Progress));
        Assert.Same(preferences, savedSnapshot.Preferences);

        CourseCatalogEntry importedCourse = savedSnapshot.Courses[1];
        Assert.Same(importedCourse, result.Course);
        Assert.Equal(courseId, importedCourse.Id);
        Assert.Equal("Curso C#", importedCourse.Title);
        Assert.Equal("D:/Courses/CSharp", importedCourse.RootPath);
        Assert.Equal(importedAt, importedCourse.ImportedAt);
        Assert.True(result.WasImported);
        Assert.Equal(CourseLibraryImportStatus.Imported, result.Status);
        Assert.Same(rejectedFile, Assert.Single(result.RejectedFiles));

        CourseCatalogItem module = Assert.Single(importedCourse.Items);
        Assert.Equal(CourseCatalogItemType.Folder, module.Type);
        CourseCatalogItem lesson = Assert.Single(module.Children);
        Assert.Equal(CourseCatalogItemType.Lesson, lesson.Type);
        Assert.Equal("Modulo/Aula 01.mp4", lesson.RelativePath);
    }

    [Fact]
    public void ImportSkipsDuplicateCourseRootWithoutReadingOrSavingAgain()
    {
        CourseCatalogEntry existingCourse = new(
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            "Curso C#",
            "D:/Courses/CSharp",
            [],
            DateTimeOffset.Parse("2026-05-01T10:00:00Z", CultureInfo.InvariantCulture));
        FakeStudyLibraryRepository repository = new(new StudyLibrarySnapshot([existingCourse], [], StudyPreferences.Default));
        CountingCourseFolderReader reader = new();
        ImportCourseToLibraryUseCase useCase = new(new ImportCourseFromFolderUseCase(reader), repository);

        CourseLibraryImportResult result = useCase.Import(new ImportCourseToLibraryCommand(
            "d:/courses/csharp/",
            Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            DateTimeOffset.Parse("2026-05-04T10:00:00Z", CultureInfo.InvariantCulture)));

        Assert.Same(existingCourse, result.Course);
        Assert.False(result.WasImported);
        Assert.Equal(CourseLibraryImportStatus.DuplicateSkipped, result.Status);
        Assert.Empty(result.RejectedFiles);
        Assert.Null(repository.SavedSnapshot);
        Assert.Equal(0, reader.ReadCount);
    }

    [Fact]
    public void CommandRejectsEmptyCourseId()
    {
        Assert.Throws<ArgumentException>(() =>
            new ImportCourseToLibraryCommand("D:/Courses/CSharp", Guid.Empty, DateTimeOffset.UtcNow));
    }

    private static CourseCatalogEntry CreateExistingCourse()
    {
        return new CourseCatalogEntry(
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            "Curso Existente",
            "D:/Courses/Existing",
            [],
            DateTimeOffset.Parse("2026-05-01T10:00:00Z", CultureInfo.InvariantCulture));
    }

    private sealed class FakeCourseFolderReader(CourseFolderSnapshot snapshot) : ICourseFolderReader
    {
        public CourseFolderSnapshot Read(ImportCourseCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            return snapshot;
        }
    }

    private sealed class CountingCourseFolderReader : ICourseFolderReader
    {
        public int ReadCount { get; private set; }

        public CourseFolderSnapshot Read(ImportCourseCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            ReadCount++;
            return new CourseFolderSnapshot("Curso C#", [], []);
        }
    }

    private sealed class FakeStudyLibraryRepository : IStudyLibraryRepository
    {
        private readonly StudyLibrarySnapshot _snapshot;

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
