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

        StudyLibrarySnapshot snapshot = _repository.Load();
        if (!PlaybackLessonCatalog.TryFindLesson(
                snapshot,
                command.CourseId,
                command.LessonId,
                out CourseCatalogEntry? course,
                out CourseCatalogItem? lesson) ||
            course is null ||
            lesson is null)
        {
            return null;
        }

        string mediaPath = ResolveMediaPath(course.RootPath, lesson.RelativePath);
        LessonProgressEntry? progress = snapshot.Progress
            .FirstOrDefault(entry => entry.LessonId == command.LessonId);

        return new LessonPlayback(
            course.Id,
            command.LessonId,
            course.Title,
            lesson.Title,
            mediaPath,
            progress?.WatchedDuration ?? TimeSpan.Zero,
            progress?.IsCompleted ?? false);
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
