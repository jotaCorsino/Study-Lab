using StudyLab.Application.Persistence;

namespace StudyLab.Application.Playback;

public sealed class RecordLessonProgressUseCase
{
    private readonly IStudyLibraryRepository _repository;

    public RecordLessonProgressUseCase(IStudyLibraryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public LessonProgressEntry? Record(RecordLessonProgressCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        StudyLibrarySnapshot snapshot = _repository.Load();
        if (!PlaybackLessonCatalog.TryFindLesson(
                snapshot,
                command.CourseId,
                command.LessonId,
                out _,
                out _))
        {
            return null;
        }

        LessonProgressEntry? existingProgress = snapshot.Progress
            .FirstOrDefault(progress => progress.LessonId == command.LessonId);
        TimeSpan watchedDuration = existingProgress is null
            ? command.WatchedDuration
            : Max(existingProgress.WatchedDuration, command.WatchedDuration);
        bool isCompleted = command.IsCompleted || existingProgress?.IsCompleted == true;
        LessonProgressEntry updatedProgress = new(command.LessonId, watchedDuration, isCompleted);
        StudyLibrarySnapshot updatedSnapshot = new(
            snapshot.Courses,
            snapshot.Progress
                .Where(progress => progress.LessonId != command.LessonId)
                .Concat([updatedProgress]),
            snapshot.Preferences);

        _repository.Save(updatedSnapshot);

        return updatedProgress;
    }

    private static TimeSpan Max(TimeSpan left, TimeSpan right)
    {
        return left >= right ? left : right;
    }
}
