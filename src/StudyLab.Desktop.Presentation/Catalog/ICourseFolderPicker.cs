namespace StudyLab.Desktop.Presentation.Catalog;

public interface ICourseFolderPicker
{
    Task<string?> PickFolderAsync();
}
