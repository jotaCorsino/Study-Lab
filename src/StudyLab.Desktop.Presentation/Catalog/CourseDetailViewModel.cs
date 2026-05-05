using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using global::StudyLab.Application.Persistence;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class CourseDetailViewModel : INotifyPropertyChanged
{
    private readonly LoadCourseDetailUseCase _loadCourseDetail;
    private readonly Guid _courseId;
    private string _title = "Carregando curso";
    private string _lessonCountText = "Nenhuma estrutura disponivel";
    private string _importedAtText = string.Empty;
    private bool _isFound;

    public CourseDetailViewModel(LoadCourseDetailUseCase loadCourseDetail, Guid courseId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        _loadCourseDetail = loadCourseDetail ?? throw new ArgumentNullException(nameof(loadCourseDetail));
        _courseId = courseId;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public Guid CourseId => _courseId;

    public ObservableCollection<CourseDetailItemViewModel> Items { get; } = [];

    public string Title
    {
        get => _title;
        private set
        {
            if (string.Equals(_title, value, StringComparison.Ordinal))
            {
                return;
            }

            _title = value;
            OnPropertyChanged();
        }
    }

    public string LessonCountText
    {
        get => _lessonCountText;
        private set
        {
            if (string.Equals(_lessonCountText, value, StringComparison.Ordinal))
            {
                return;
            }

            _lessonCountText = value;
            OnPropertyChanged();
        }
    }

    public string ImportedAtText
    {
        get => _importedAtText;
        private set
        {
            if (string.Equals(_importedAtText, value, StringComparison.Ordinal))
            {
                return;
            }

            _importedAtText = value;
            OnPropertyChanged();
        }
    }

    public bool IsFound
    {
        get => _isFound;
        private set
        {
            if (_isFound == value)
            {
                return;
            }

            _isFound = value;
            OnPropertyChanged();
        }
    }

    public bool HasItems => Items.Count > 0;

    public void Load()
    {
        CourseDetail? detail = _loadCourseDetail.Load(_courseId);

        Items.Clear();
        if (detail is null)
        {
            IsFound = false;
            Title = "Curso nao encontrado";
            LessonCountText = "Nenhuma estrutura disponivel";
            ImportedAtText = string.Empty;
            OnPropertyChanged(nameof(HasItems));
            return;
        }

        IsFound = true;
        Title = detail.Title;
        LessonCountText = detail.LessonCount == 1 ? "1 aula" : $"{detail.LessonCount} aulas";
        ImportedAtText = $"Importado em {detail.ImportedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)}";

        foreach (CourseDetailItemViewModel item in detail.Items.Select(CourseDetailItemViewModel.FromDetailItem))
        {
            Items.Add(item);
        }

        OnPropertyChanged(nameof(HasItems));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
