namespace StudyLab.Application.Persistence;

public sealed class StudyPreferences
{
    public StudyPreferences(decimal defaultPlaybackSpeed, bool introSkipEnabled, TimeSpan introSkipDuration)
    {
        if (defaultPlaybackSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(defaultPlaybackSpeed), defaultPlaybackSpeed, "Playback speed must be greater than zero.");
        }

        if (introSkipDuration < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(introSkipDuration), introSkipDuration, "Intro skip duration cannot be negative.");
        }

        DefaultPlaybackSpeed = defaultPlaybackSpeed;
        IntroSkipEnabled = introSkipEnabled;
        IntroSkipDuration = introSkipDuration;
    }

    public decimal DefaultPlaybackSpeed { get; }

    public bool IntroSkipEnabled { get; }

    public TimeSpan IntroSkipDuration { get; }

    public static StudyPreferences Default { get; } = new(1.0m, introSkipEnabled: false, TimeSpan.Zero);
}
