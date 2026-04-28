namespace StudyLab.Application.Persistence;

public interface IStudyLibraryRepository
{
    StudyLibrarySnapshot Load();

    void Save(StudyLibrarySnapshot snapshot);
}
