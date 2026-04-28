namespace StudyLab.Domain.Study;

public sealed class MonthlyCredit
{
    private readonly Dictionary<DateOnly, StudyDayPerformance> _days = [];

    private MonthlyCredit(int year, int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), month, "Month must be between 1 and 12.");
        }

        Year = year;
        Month = month;
    }

    public int Year { get; }

    public int Month { get; }

    public IReadOnlyCollection<StudyDayPerformance> Days => _days.Values;

    public static MonthlyCredit For(int year, int month)
    {
        return new MonthlyCredit(year, month);
    }

    public void RecordStudy(DateOnly date, TimeSpan targetDuration, TimeSpan studiedDuration)
    {
        EnsureDateIsTracked(date);

        _days[date] = StudyDayPerformance.Record(date, targetDuration, studiedDuration);
    }

    public StudyDayPerformance GetDay(DateOnly date)
    {
        EnsureDateIsTracked(date);

        if (!_days.TryGetValue(date, out StudyDayPerformance? day))
        {
            throw new KeyNotFoundException("No study performance was recorded for the requested date.");
        }

        return day;
    }

    public void ApplyAutomaticAbatements(DateOnly asOfDate)
    {
        EnsureDateIsTracked(asOfDate);

        TimeSpan remainingCredit = GetRemainingCredit(asOfDate);
        if (remainingCredit == TimeSpan.Zero)
        {
            return;
        }

        foreach (StudyDayPerformance day in _days.Values
            .Where(day => day.Date < asOfDate && day.RemainingDeficit > TimeSpan.Zero)
            .OrderBy(day => day.Date))
        {
            TimeSpan compensation = day.RemainingDeficit < remainingCredit
                ? day.RemainingDeficit
                : remainingCredit;

            _days[day.Date] = day.WithAdditionalCompensation(compensation);
            remainingCredit -= compensation;

            if (remainingCredit == TimeSpan.Zero)
            {
                return;
            }
        }
    }

    public TimeSpan GetRemainingCredit(DateOnly asOfDate)
    {
        EnsureDateIsTracked(asOfDate);

        TimeSpan generatedCredit = SumDurations(_days.Values
            .Where(day => day.Date <= asOfDate)
            .Select(day => day.ExtraDuration));

        TimeSpan consumedCredit = SumDurations(_days.Values
            .Select(day => day.CompensatedDuration));

        TimeSpan remainingCredit = generatedCredit - consumedCredit;
        return remainingCredit > TimeSpan.Zero ? remainingCredit : TimeSpan.Zero;
    }

    private void EnsureDateIsTracked(DateOnly date)
    {
        if (date.Year != Year || date.Month != Month)
        {
            throw new ArgumentOutOfRangeException(nameof(date), date, "Date must belong to the tracked month.");
        }
    }

    private static TimeSpan SumDurations(IEnumerable<TimeSpan> durations)
    {
        long ticks = durations.Sum(duration => duration.Ticks);
        return TimeSpan.FromTicks(ticks);
    }
}
