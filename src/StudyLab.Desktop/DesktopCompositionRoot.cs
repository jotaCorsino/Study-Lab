using global::StudyLab.Application.Persistence;
using global::StudyLab.Infrastructure.Persistence;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop;

internal static class DesktopCompositionRoot
{
    public static CatalogViewModel CreateCatalogViewModel()
    {
        string libraryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StudyLab",
            "library.json");

        IStudyLibraryRepository repository = new JsonStudyLibraryRepository(libraryPath);
        return new CatalogViewModel(new LoadStudyLibraryUseCase(repository));
    }
}
