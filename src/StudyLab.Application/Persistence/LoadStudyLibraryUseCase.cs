namespace StudyLab.Application.Persistence;

public sealed class LoadStudyLibraryUseCase
{
    private readonly IStudyLibraryRepository _repository;

    public LoadStudyLibraryUseCase(IStudyLibraryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public StudyLibrarySnapshot Load()
    {
        return _repository.Load();
    }
}
