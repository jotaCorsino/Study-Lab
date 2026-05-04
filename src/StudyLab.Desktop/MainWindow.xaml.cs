using Microsoft.UI.Xaml;

namespace StudyLab.Desktop;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        AppWindow.SetIcon("Assets/AppIcon.ico");
        RootFrame.Navigate(typeof(MainPage), DesktopCompositionRoot.CreateCatalogViewModel(this));
    }
}
