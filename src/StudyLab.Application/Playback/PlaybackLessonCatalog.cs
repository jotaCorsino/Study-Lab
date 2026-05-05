using StudyLab.Application.Persistence;

namespace StudyLab.Application.Playback;

internal static class PlaybackLessonCatalog
{
    public static bool TryFindLesson(
        StudyLibrarySnapshot snapshot,
        Guid courseId,
        Guid lessonId,
        out CourseCatalogEntry? course,
        out CourseCatalogItem? lesson)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        CourseCatalogEntry? foundCourse = snapshot.Courses.FirstOrDefault(entry => entry.Id == courseId);
        CourseCatalogItem? foundLesson = foundCourse is null
            ? null
            : EnumerateLessons(foundCourse.Items).FirstOrDefault(item => GetLessonId(foundCourse.Id, item) == lessonId);
        course = foundCourse;
        lesson = foundLesson;

        return lesson is not null;
    }

    public static Guid GetLessonId(Guid courseId, CourseCatalogItem item)
    {
        if (item.Type != CourseCatalogItemType.Lesson || item.RelativePath is null)
        {
            throw new InvalidDataException("Stored catalog item is not a playable lesson.");
        }

        return LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, item.RelativePath);
    }

    private static IEnumerable<CourseCatalogItem> EnumerateLessons(IEnumerable<CourseCatalogItem> items)
    {
        foreach (CourseCatalogItem item in items)
        {
            if (item.Type == CourseCatalogItemType.Lesson)
            {
                yield return item;
            }

            foreach (CourseCatalogItem child in EnumerateLessons(item.Children))
            {
                yield return child;
            }
        }
    }
}
