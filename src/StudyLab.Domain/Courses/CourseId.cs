namespace StudyLab.Domain.Courses;

public readonly record struct CourseId
{
    public CourseId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Course id cannot be empty.", nameof(value));
        }

        Value = value;
    }

    public Guid Value { get; }

    public static CourseId New()
    {
        return new CourseId(Guid.NewGuid());
    }
}
