using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using StudyLab.Desktop.Presentation.Playback;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace StudyLab.Desktop;

public sealed partial class LessonPlayerPage : Page
{
    private TimeSpan? _pendingResumePosition;

    public LessonPlayerPage()
    {
        InitializeComponent();
    }

    public LessonPlayerViewModel? ViewModel { get; private set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not LessonPlayerViewModel viewModel)
        {
            throw new InvalidOperationException("LessonPlayerPage requires a lesson player view model.");
        }

        ViewModel = viewModel;
        DataContext = ViewModel;
        ViewModel.Load();

        StatusInfoBar.Severity = ViewModel.HasError
            ? InfoBarSeverity.Error
            : InfoBarSeverity.Informational;

        if (ViewModel.IsLoaded && ViewModel.MediaPath is not null)
        {
            PrepareResumePosition();
            LessonMediaPlayer.Source = MediaSource.CreateFromUri(new Uri(ViewModel.MediaPath));
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        if (LessonMediaPlayer.MediaPlayer is not null)
        {
            LessonMediaPlayer.MediaPlayer.MediaOpened -= LessonMediaPlayer_MediaOpened;
        }

        _pendingResumePosition = null;
        LessonMediaPlayer.MediaPlayer?.Pause();
        LessonMediaPlayer.Source = null;
        base.OnNavigatedFrom(e);
    }

    private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (Frame.CanGoBack)
        {
            Frame.GoBack();
        }
    }

    private void MarkCompletedButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel is null)
        {
            return;
        }

        TimeSpan currentPosition = LessonMediaPlayer.MediaPlayer?.PlaybackSession.Position ?? TimeSpan.Zero;
        ViewModel.MarkCompleted(currentPosition);
        StatusInfoBar.Severity = ViewModel.HasError
            ? InfoBarSeverity.Error
            : InfoBarSeverity.Success;
    }

    private void PrepareResumePosition()
    {
        if (LessonMediaPlayer.MediaPlayer is null)
        {
            return;
        }

        LessonMediaPlayer.MediaPlayer.MediaOpened -= LessonMediaPlayer_MediaOpened;
        if (ViewModel?.ShouldResumePlayback != true)
        {
            _pendingResumePosition = null;
            return;
        }

        _pendingResumePosition = ViewModel.ResumePosition;
        LessonMediaPlayer.MediaPlayer.MediaOpened += LessonMediaPlayer_MediaOpened;
    }

    private void LessonMediaPlayer_MediaOpened(MediaPlayer sender, object args)
    {
        DispatcherQueue.TryEnqueue(ApplyPendingResumePosition);
    }

    private void ApplyPendingResumePosition()
    {
        if (_pendingResumePosition is not TimeSpan resumePosition ||
            LessonMediaPlayer.MediaPlayer is null)
        {
            return;
        }

        LessonMediaPlayer.MediaPlayer.PlaybackSession.Position = resumePosition;
        _pendingResumePosition = null;
    }
}
