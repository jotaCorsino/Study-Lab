using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    public CatalogViewModel? ViewModel { get; private set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not CatalogViewModel viewModel)
        {
            throw new InvalidOperationException("MainPage requires a catalog view model.");
        }

        ViewModel = viewModel;
        DataContext = ViewModel;
        ViewModel.Load();
    }

    private async void ImportCourseButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (ViewModel is null)
        {
            return;
        }

        ImportCourseButton.IsEnabled = false;
        try
        {
            await ViewModel.ImportCourseAsync();
        }
        finally
        {
            ImportCourseButton.IsEnabled = true;
        }
    }

    private void CourseListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is not CatalogCourseViewModel course || Frame is null)
        {
            return;
        }

        Frame.Navigate(typeof(CourseDetailPage), DesktopCompositionRoot.CreateCourseDetailViewModel(course.Id));
    }
}
