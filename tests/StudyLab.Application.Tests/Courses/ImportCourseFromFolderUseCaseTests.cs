using StudyLab.Application.Courses.Importing;

namespace StudyLab.Application.Tests.Courses;

public sealed class ImportCourseFromFolderUseCaseTests
{
    [Fact]
    public void ImportBuildsHierarchicalTreeFromRelativeVideoPaths()
    {
        FakeCourseFolderReader reader = new(new CourseFolderSnapshot(
            "Curso C#",
            [
                new CourseFileCandidate("Materia/Modulo/Topico/Aula 01.mp4"),
                new CourseFileCandidate("Materia/Modulo/Topico/Aula 02.mkv")
            ],
            []));
        ImportCourseFromFolderUseCase useCase = new(reader);

        CourseImportResult result = useCase.Import(new ImportCourseCommand("C:/courses/curso-csharp"));

        ImportedCourseItem subject = Assert.Single(result.Course.Items);
        Assert.Equal("Materia", subject.Title);
        ImportedCourseItem module = Assert.Single(subject.Children);
        Assert.Equal("Modulo", module.Title);
        ImportedCourseItem topic = Assert.Single(module.Children);
        Assert.Equal("Topico", topic.Title);
        Assert.Equal(2, topic.Children.Count);
        Assert.All(topic.Children, lesson => Assert.Equal(ImportedCourseItemType.Lesson, lesson.Type));
    }

    [Fact]
    public void ImportPreservesRejectedFilesFromReader()
    {
        RejectedCourseFile rejectedFile = new("Notas.txt", CourseFileRejectionReason.UnsupportedExtension);
        FakeCourseFolderReader reader = new(new CourseFolderSnapshot(
            "Curso C#",
            [new CourseFileCandidate("Aula 01.mp4")],
            [rejectedFile]));
        ImportCourseFromFolderUseCase useCase = new(reader);

        CourseImportResult result = useCase.Import(new ImportCourseCommand("C:/courses/curso-csharp"));

        Assert.Same(rejectedFile, Assert.Single(result.RejectedFiles));
    }

    [Fact]
    public void CommandRejectsEmptyRootPath()
    {
        Assert.Throws<ArgumentException>(() => new ImportCourseCommand(" "));
    }

    private sealed class FakeCourseFolderReader(CourseFolderSnapshot snapshot) : ICourseFolderReader
    {
        public CourseFolderSnapshot Read(ImportCourseCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            return snapshot;
        }
    }
}
