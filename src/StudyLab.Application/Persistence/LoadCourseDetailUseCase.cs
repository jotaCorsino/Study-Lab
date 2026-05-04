namespace StudyLab.Application.Persistence;

public sealed class LoadCourseDetailUseCase
{
    private readonly IStudyLibraryRepository _repository;

    public LoadCourseDetailUseCase(IStudyLibraryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public CourseDetail? Load(Guid courseId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        CourseCatalogEntry? course = _repository.Load()
            .Courses
            .FirstOrDefault(entry => entry.Id == courseId);

        return course is null
            ? null
            : new CourseDetail(
                course.Id,
                course.Title,
                course.Items.Select(ToDetailItem),
                course.ImportedAt);
    }

    private static CourseDetailItem ToDetailItem(CourseCatalogItem item)
    {
        return new CourseDetailItem(
            item.Type,
            item.Title,
            item.RelativePath,
            item.Children.Select(ToDetailItem));
    }
}
