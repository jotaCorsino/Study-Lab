using global::StudyLab.Application.Courses.Importing;
using global::StudyLab.Application.Persistence;
using global::StudyLab.Infrastructure.Courses.Importing;
using global::StudyLab.Infrastructure.Persistence;
using StudyLab.Desktop.Presentation.Catalog;
using Microsoft.UI.Xaml;

namespace StudyLab.Desktop;

internal static class DesktopCompositionRoot
{
    public static CatalogViewModel CreateCatalogViewModel(Window owner)
    {
        IStudyLibraryRepository repository = CreateRepository();
        LocalCourseFolderReader reader = new();

        return new CatalogViewModel(
            new LoadStudyLibraryUseCase(repository),
            new ImportCourseToLibraryUseCase(new ImportCourseFromFolderUseCase(reader), repository),
            new WinUiCourseFolderPicker(owner));
    }

    public static CourseDetailViewModel CreateCourseDetailViewModel(Guid courseId)
    {
        return new CourseDetailViewModel(
            new LoadCourseDetailUseCase(CreateRepository()),
            courseId);
    }

    private static JsonStudyLibraryRepository CreateRepository()
    {
        string libraryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StudyLab",
            "library.json");

        return new JsonStudyLibraryRepository(libraryPath);
    }
}
