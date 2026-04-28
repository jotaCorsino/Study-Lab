namespace StudyLab.Application.Persistence;

public sealed class SaveStudyLibraryUseCase
{
    private readonly IStudyLibraryRepository _repository;

    public SaveStudyLibraryUseCase(IStudyLibraryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public void Save(StudyLibrarySnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        _repository.Save(snapshot);
    }
}
