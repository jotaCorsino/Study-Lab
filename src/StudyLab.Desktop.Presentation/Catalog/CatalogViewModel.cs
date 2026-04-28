using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using global::StudyLab.Application.Persistence;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class CatalogViewModel : INotifyPropertyChanged
{
    private readonly LoadStudyLibraryUseCase _loadStudyLibrary;

    public CatalogViewModel(LoadStudyLibraryUseCase loadStudyLibrary)
    {
        _loadStudyLibrary = loadStudyLibrary ?? throw new ArgumentNullException(nameof(loadStudyLibrary));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<CatalogCourseViewModel> Courses { get; } = [];

    public bool HasCourses => Courses.Count > 0;

    public string CourseCountText => Courses.Count switch
    {
        0 => "Nenhum curso importado",
        1 => "1 curso no catalogo",
        _ => $"{Courses.Count} cursos no catalogo"
    };

    public void Load()
    {
        StudyLibrarySnapshot snapshot = _loadStudyLibrary.Load();

        Courses.Clear();
        foreach (CatalogCourseViewModel course in snapshot.Courses.Select(CatalogCourseViewModel.FromEntry))
        {
            Courses.Add(course);
        }

        OnPropertyChanged(nameof(HasCourses));
        OnPropertyChanged(nameof(CourseCountText));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
