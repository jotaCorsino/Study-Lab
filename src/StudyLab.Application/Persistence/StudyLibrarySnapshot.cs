namespace StudyLab.Application.Persistence;

public sealed class StudyLibrarySnapshot
{
    public StudyLibrarySnapshot(
        IEnumerable<CourseCatalogEntry> courses,
        IEnumerable<LessonProgressEntry> progress,
        StudyPreferences preferences)
    {
        Courses = courses?.ToArray() ?? throw new ArgumentNullException(nameof(courses));
        Progress = progress?.ToArray() ?? throw new ArgumentNullException(nameof(progress));
        Preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
    }

    public IReadOnlyList<CourseCatalogEntry> Courses { get; }

    public IReadOnlyList<LessonProgressEntry> Progress { get; }

    public StudyPreferences Preferences { get; }

    public static StudyLibrarySnapshot Empty { get; } = new(
        [],
        [],
        StudyPreferences.Default);
}
