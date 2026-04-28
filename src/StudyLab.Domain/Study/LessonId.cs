namespace StudyLab.Domain.Study;

public readonly record struct LessonId
{
    public LessonId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Lesson id cannot be empty.", nameof(value));
        }

        Value = value;
    }

    public Guid Value { get; }

    public static LessonId New()
    {
        return new LessonId(Guid.NewGuid());
    }
}
