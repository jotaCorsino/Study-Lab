using global::StudyLab.Application.Courses.Importing;

namespace StudyLab.Desktop.Presentation.Catalog;

public sealed class RejectedCourseFileViewModel
{
    private RejectedCourseFileViewModel(string location, string reasonText)
    {
        Location = location;
        ReasonText = reasonText;
    }

    public string Location { get; }

    public string ReasonText { get; }

    public static RejectedCourseFileViewModel FromRejectedFile(RejectedCourseFile rejectedFile)
    {
        ArgumentNullException.ThrowIfNull(rejectedFile);

        return new RejectedCourseFileViewModel(
            rejectedFile.RelativePath,
            ToReasonText(rejectedFile.Reason));
    }

    private static string ToReasonText(CourseFileRejectionReason reason)
    {
        return reason switch
        {
            CourseFileRejectionReason.UnsupportedExtension => "Extensao nao suportada",
            CourseFileRejectionReason.PathOutsideRoot => "Fora da pasta selecionada",
            CourseFileRejectionReason.ReparsePointNotAllowed => "Atalho ou link nao permitido",
            _ => "Arquivo ignorado"
        };
    }
}
