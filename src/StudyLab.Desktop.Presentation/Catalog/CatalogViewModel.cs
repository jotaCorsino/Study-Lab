using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using global::StudyLab.Application.Courses.Importing;
using global::StudyLab.Application.Persistence;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class CatalogViewModel : INotifyPropertyChanged
{
    private readonly LoadStudyLibraryUseCase _loadStudyLibrary;
    private readonly ImportCourseToLibraryUseCase _importCourseToLibrary;
    private readonly ICourseFolderPicker _folderPicker;
    private readonly Func<Guid> _newCourseId;
    private readonly Func<DateTimeOffset> _getImportedAt;
    private bool _isImporting;
    private string _statusMessage = "Pronto para importar cursos";

    public CatalogViewModel(
        LoadStudyLibraryUseCase loadStudyLibrary,
        ImportCourseToLibraryUseCase importCourseToLibrary,
        ICourseFolderPicker folderPicker,
        Func<Guid>? newCourseId = null,
        Func<DateTimeOffset>? getImportedAt = null)
    {
        _loadStudyLibrary = loadStudyLibrary ?? throw new ArgumentNullException(nameof(loadStudyLibrary));
        _importCourseToLibrary = importCourseToLibrary ?? throw new ArgumentNullException(nameof(importCourseToLibrary));
        _folderPicker = folderPicker ?? throw new ArgumentNullException(nameof(folderPicker));
        _newCourseId = newCourseId ?? Guid.NewGuid;
        _getImportedAt = getImportedAt ?? (() => DateTimeOffset.UtcNow);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<CatalogCourseViewModel> Courses { get; } = [];

    public ObservableCollection<RejectedCourseFileViewModel> RejectedFiles { get; } = [];

    public bool HasCourses => Courses.Count > 0;

    public bool HasRejectedFiles => RejectedFiles.Count > 0;

    public string RejectedFilesSummary => RejectedFiles.Count switch
    {
        0 => "Nenhum arquivo ignorado",
        1 => "1 arquivo ignorado",
        _ => $"{RejectedFiles.Count} arquivos ignorados"
    };

    public bool IsImporting
    {
        get => _isImporting;
        private set
        {
            if (_isImporting == value)
            {
                return;
            }

            _isImporting = value;
            OnPropertyChanged();
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set
        {
            if (string.Equals(_statusMessage, value, StringComparison.Ordinal))
            {
                return;
            }

            _statusMessage = value;
            OnPropertyChanged();
        }
    }

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

    public async Task ImportCourseAsync()
    {
        if (IsImporting)
        {
            return;
        }

        try
        {
            IsImporting = true;
            StatusMessage = "Selecionando pasta do curso";

            string? selectedPath = await _folderPicker.PickFolderAsync();
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                ClearRejectedFiles();
                StatusMessage = "Importacao cancelada";
                return;
            }

            CourseLibraryImportResult result = _importCourseToLibrary.Import(new ImportCourseToLibraryCommand(
                selectedPath,
                _newCourseId(),
                _getImportedAt()));

            Load();
            UpdateRejectedFiles(result.RejectedFiles);
            StatusMessage = result.Status switch
            {
                CourseLibraryImportStatus.DuplicateSkipped => "Curso ja importado",
                _ when result.RejectedFiles.Count == 0 => "Curso importado com sucesso",
                _ => $"Curso importado com {result.RejectedFiles.Count} arquivos ignorados"
            };
        }
        catch (Exception exception) when (IsSafeImportFailure(exception))
        {
            ClearRejectedFiles();
            StatusMessage = "Nao foi possivel importar o curso selecionado.";
        }
        finally
        {
            IsImporting = false;
        }
    }

    private static bool IsSafeImportFailure(Exception exception)
    {
        return exception is ArgumentException
            or IOException
            or InvalidDataException
            or UnauthorizedAccessException;
    }

    private void UpdateRejectedFiles(IEnumerable<RejectedCourseFile> rejectedFiles)
    {
        RejectedFiles.Clear();
        foreach (RejectedCourseFileViewModel rejectedFile in rejectedFiles.Select(RejectedCourseFileViewModel.FromRejectedFile))
        {
            RejectedFiles.Add(rejectedFile);
        }

        OnPropertyChanged(nameof(HasRejectedFiles));
        OnPropertyChanged(nameof(RejectedFilesSummary));
    }

    private void ClearRejectedFiles()
    {
        if (RejectedFiles.Count == 0)
        {
            return;
        }

        RejectedFiles.Clear();
        OnPropertyChanged(nameof(HasRejectedFiles));
        OnPropertyChanged(nameof(RejectedFilesSummary));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
