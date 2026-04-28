using StudyLab.Domain.Common;

namespace StudyLab.Domain.Study;

public sealed class StudySession
{
    public StudySession(DateOnly date, TimeSpan studiedDuration)
    {
        Date = date;
        StudiedDuration = Guard.NonNegativeDuration(studiedDuration, nameof(studiedDuration));
    }

    public DateOnly Date { get; }

    public TimeSpan StudiedDuration { get; }
}
