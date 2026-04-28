namespace StudyLab.Domain.Common;

internal static class Guard
{
    public static string RequiredText(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be empty.", parameterName);
        }

        return value.Trim();
    }

    public static TimeSpan PositiveDuration(TimeSpan value, string parameterName)
    {
        if (value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Duration must be greater than zero.");
        }

        return value;
    }

    public static TimeSpan NonNegativeDuration(TimeSpan value, string parameterName)
    {
        if (value < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Duration cannot be negative.");
        }

        return value;
    }
}
