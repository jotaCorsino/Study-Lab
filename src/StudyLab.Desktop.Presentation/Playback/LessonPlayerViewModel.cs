using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyLab.Application.Persistence;
using StudyLab.Application.Playback;

namespace StudyLab.Desktop.Presentation.Playback;

public sealed class LessonPlayerViewModel : INotifyPropertyChanged
{
    private readonly LoadLessonPlaybackUseCase _loadLessonPlayback;
    private readonly RecordLessonProgressUseCase _recordLessonProgress;
    private readonly Guid _courseId;
    private readonly Guid _lessonId;
    private string _courseTitle = "Carregando curso";
    private string _lessonTitle = "Carregando aula";
    private string _statusMessage = string.Empty;
    private string _progressText = "Progresso nao iniciado";
    private string? _mediaPath;
    private bool _isLoaded;
    private bool _hasError;
    private bool _isCompleted;

    public LessonPlayerViewModel(
        LoadLessonPlaybackUseCase loadLessonPlayback,
        RecordLessonProgressUseCase recordLessonProgress,
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
        _recordLessonProgress = recordLessonProgress ?? throw new ArgumentNullException(nameof(recordLessonProgress));
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

    public string ProgressText
    {
        get => _progressText;
        private set => SetField(ref _progressText, value);
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

    public bool IsCompleted
    {
        get => _isCompleted;
        private set
        {
            if (SetField(ref _isCompleted, value))
            {
                OnPropertyChanged(nameof(CanMarkCompleted));
            }
        }
    }

    public bool CanMarkCompleted => IsLoaded && !HasError && !IsCompleted;

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
            IsCompleted = playback.IsCompleted;
            ProgressText = FormatProgress(playback.WatchedDuration, playback.IsCompleted);
            StatusMessage = "Pronto para reproduzir";
            IsLoaded = true;
            HasError = false;
            OnPropertyChanged(nameof(CanMarkCompleted));
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

    public void MarkCompleted(TimeSpan currentPosition)
    {
        if (!CanMarkCompleted)
        {
            return;
        }

        LessonProgressEntry? progress = _recordLessonProgress.Record(new RecordLessonProgressCommand(
            _courseId,
            _lessonId,
            currentPosition,
            isCompleted: true));
        if (progress is null)
        {
            ShowSafeError("Aula nao encontrada");
            return;
        }

        IsCompleted = progress.IsCompleted;
        ProgressText = FormatProgress(progress.WatchedDuration, progress.IsCompleted);
        StatusMessage = "Aula concluida";
    }

    private void ShowSafeError(string message)
    {
        MediaPath = null;
        IsLoaded = false;
        HasError = true;
        IsCompleted = false;
        ProgressText = "Progresso indisponivel";
        StatusMessage = message;
        OnPropertyChanged(nameof(CanMarkCompleted));
    }

    private static string FormatProgress(TimeSpan watchedDuration, bool isCompleted)
    {
        if (isCompleted)
        {
            return "Concluida";
        }

        if (watchedDuration == TimeSpan.Zero)
        {
            return "Progresso nao iniciado";
        }

        int totalMinutes = Math.Max(1, (int)Math.Round(watchedDuration.TotalMinutes, MidpointRounding.AwayFromZero));
        return totalMinutes == 1 ? "1 min assistido" : $"{totalMinutes} min assistidos";
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
