using Microsoft.UI.Xaml.Controls;
using StudyLab.Desktop.Presentation.Catalog;

namespace StudyLab.Desktop;

public sealed partial class MainPage : Page
{
    public MainPage()
        : this(DesktopCompositionRoot.CreateCatalogViewModel())
    {
    }

    internal MainPage(CatalogViewModel viewModel)
    {
        ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.Load();
    }

    public CatalogViewModel ViewModel { get; }
}
