using StudyLab.Application.Persistence;

namespace StudyLab.Application.Playback;

public sealed class LoadLessonPlaybackUseCase
{
    private static readonly HashSet<string> SupportedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4",
        ".mkv",
        ".avi",
        ".mov",
        ".wmv",
        ".webm",
        ".flv",
        ".m4v"
    };

    private readonly IStudyLibraryRepository _repository;

    public LoadLessonPlaybackUseCase(IStudyLibraryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public LessonPlayback? Load(LoadLessonPlaybackCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        CourseCatalogEntry? course = _repository.Load()
            .Courses
            .FirstOrDefault(entry => entry.Id == command.CourseId);

        if (course is null)
        {
            return null;
        }

        CourseCatalogItem? lesson = EnumerateLessons(course.Items)
            .FirstOrDefault(item => GetLessonId(course.Id, item) == command.LessonId);

        if (lesson is null)
        {
            return null;
        }

        string mediaPath = ResolveMediaPath(course.RootPath, lesson.RelativePath);

        return new LessonPlayback(
            course.Id,
            command.LessonId,
            course.Title,
            lesson.Title,
            mediaPath);
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

    private static Guid GetLessonId(Guid courseId, CourseCatalogItem item)
    {
        if (item.Type != CourseCatalogItemType.Lesson || item.RelativePath is null)
        {
            throw new InvalidDataException("Stored catalog item is not a playable lesson.");
        }

        return LessonPlaybackIdentity.FromCourseAndRelativePath(courseId, item.RelativePath);
    }

    private static string ResolveMediaPath(string rootPath, string? relativePath)
    {
        if (relativePath is null)
        {
            throw new InvalidDataException("Stored lesson does not have a media path.");
        }

        string normalizedRelativePath = LessonPlaybackIdentity.NormalizeRelativePath(relativePath);
        if (!SupportedVideoExtensions.Contains(Path.GetExtension(normalizedRelativePath)))
        {
            throw new InvalidDataException("Stored lesson media is not a supported video file.");
        }

        string fullRootPath = Path.GetFullPath(rootPath);
        string candidatePath = Path.GetFullPath(Path.Combine(
            fullRootPath,
            normalizedRelativePath.Replace('/', Path.DirectorySeparatorChar)));
        string rootWithSeparator = EnsureTrailingSeparator(fullRootPath);

        if (!candidatePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Stored lesson media must stay inside the course root.");
        }

        return candidatePath;
    }

    private static string EnsureTrailingSeparator(string path)
    {
        return path.EndsWith(Path.DirectorySeparatorChar)
            ? path
            : path + Path.DirectorySeparatorChar;
    }
}
