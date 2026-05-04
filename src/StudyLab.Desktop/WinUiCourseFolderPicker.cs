using Microsoft.UI.Xaml;
using StudyLab.Desktop.Presentation.Catalog;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace StudyLab.Desktop;

internal sealed class WinUiCourseFolderPicker : ICourseFolderPicker
{
    private readonly Window _owner;

    public WinUiCourseFolderPicker(Window owner)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public async Task<string?> PickFolderAsync()
    {
        FolderPicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.VideosLibrary
        };
        picker.FileTypeFilter.Add("*");

        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(_owner));

        StorageFolder? folder = await picker.PickSingleFolderAsync();
        return folder?.Path;
    }
}
