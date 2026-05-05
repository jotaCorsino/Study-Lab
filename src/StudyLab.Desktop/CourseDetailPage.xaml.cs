using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop;

public sealed partial class CourseDetailPage : Page
{
    public CourseDetailPage()
    {
        InitializeComponent();
    }

    public CourseDetailViewModel? ViewModel { get; private set; }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not CourseDetailViewModel viewModel)
        {
            throw new InvalidOperationException("CourseDetailPage requires a course detail view model.");
        }

        ViewModel = viewModel;
        DataContext = ViewModel;
        ViewModel.Load();
        RebuildTree();
    }

    private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (Frame.CanGoBack)
        {
            Frame.GoBack();
        }
    }

    private void CourseTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
    {
        if (ViewModel is null ||
            args.InvokedItem is not CourseDetailItemViewModel item ||
            !item.CanOpenLesson ||
            item.LessonId is not Guid lessonId)
        {
            return;
        }

        Frame.Navigate(typeof(LessonPlayerPage), DesktopCompositionRoot.CreateLessonPlayerViewModel(ViewModel.CourseId, lessonId));
    }

    private void RebuildTree()
    {
        CourseTreeView.RootNodes.Clear();

        if (ViewModel is null)
        {
            return;
        }

        foreach (CourseDetailItemViewModel item in ViewModel.Items)
        {
            CourseTreeView.RootNodes.Add(CreateNode(item));
        }
    }

    private static TreeViewNode CreateNode(CourseDetailItemViewModel item)
    {
        TreeViewNode node = new()
        {
            Content = item,
            IsExpanded = true
        };

        foreach (CourseDetailItemViewModel child in item.Children)
        {
            node.Children.Add(CreateNode(child));
        }

        return node;
    }
}
