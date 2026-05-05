using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyLab.Application.Playback;

namespace StudyLab.Desktop.Presentation.Playback;

public sealed class LessonPlayerViewModel : INotifyPropertyChanged
{
    private readonly LoadLessonPlaybackUseCase _loadLessonPlayback;
    private readonly Guid _courseId;
    private readonly Guid _lessonId;
    private string _courseTitle = "Carregando curso";
    private string _lessonTitle = "Carregando aula";
    private string _statusMessage = string.Empty;
    private string? _mediaPath;
    private bool _isLoaded;
    private bool _hasError;

    public LessonPlayerViewModel(
        LoadLessonPlaybackUseCase loadLessonPlayback,
        Guid courseId,
        Guid lessonId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(courseId));
        }

        if (lessonId == Guid.Empty)
        {
            throw new ArgumentException("Lesson id cannot be empty.", nameof(lessonId));
        }

        _loadLessonPlayback = loadLessonPlayback ?? throw new ArgumentNullException(nameof(loadLessonPlayback));
        _courseId = courseId;
        _lessonId = lessonId;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string CourseTitle
    {
        get => _courseTitle;
        private set => SetField(ref _courseTitle, value);
    }

    public string LessonTitle
    {
        get => _lessonTitle;
        private set => SetField(ref _lessonTitle, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetField(ref _statusMessage, value);
    }

    public string? MediaPath
    {
        get => _mediaPath;
        private set => SetField(ref _mediaPath, value);
    }

    public bool IsLoaded
    {
        get => _isLoaded;
        private set => SetField(ref _isLoaded, value);
    }

    public bool HasError
    {
        get => _hasError;
        private set => SetField(ref _hasError, value);
    }

    public void Load()
    {
        try
        {
            LessonPlayback? playback = _loadLessonPlayback.Load(new LoadLessonPlaybackCommand(_courseId, _lessonId));
            if (playback is null)
            {
                ShowSafeError("Aula nao encontrada");
                return;
            }

            CourseTitle = playback.CourseTitle;
            LessonTitle = playback.LessonTitle;
            MediaPath = playback.MediaPath;
            StatusMessage = "Pronto para reproduzir";
            IsLoaded = true;
            HasError = false;
        }
        catch (InvalidDataException)
        {
            ShowSafeError("Nao foi possivel abrir esta aula com seguranca");
        }
        catch (UnauthorizedAccessException)
        {
            ShowSafeError("Nao foi possivel abrir esta aula com seguranca");
        }
    }

    private void ShowSafeError(string message)
    {
        MediaPath = null;
        IsLoaded = false;
        HasError = true;
        StatusMessage = message;
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
