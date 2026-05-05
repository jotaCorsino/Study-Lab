using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using StudyLab.Desktop.Presentation.Playback;
using Windows.Media.Core;

namespace StudyLab.Desktop;

public sealed partial class LessonPlayerPage : Page
{
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
            LessonMediaPlayer.Source = MediaSource.CreateFromUri(new Uri(ViewModel.MediaPath));
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
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
}
