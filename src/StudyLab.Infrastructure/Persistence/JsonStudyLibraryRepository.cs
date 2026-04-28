using System.Text.Json;
using StudyLab.Application.Persistence;

namespace StudyLab.Infrastructure.Persistence;

public sealed class JsonStudyLibraryRepository : IStudyLibraryRepository
{
    private const int CurrentSchemaVersion = 1;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.General)
    {
        WriteIndented = true
    };

    private readonly string _storageFilePath;

    public JsonStudyLibraryRepository(string storageFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageFilePath);

        _storageFilePath = Path.GetFullPath(storageFilePath);
    }

    public StudyLibrarySnapshot Load()
    {
        if (!File.Exists(_storageFilePath))
        {
            return StudyLibrarySnapshot.Empty;
        }

        try
        {
            string json = File.ReadAllText(_storageFilePath);
            LibraryDocument document = JsonSerializer.Deserialize<LibraryDocument>(json, JsonOptions)
                ?? throw new InvalidDataException("Study library file is empty.");

            if (document.SchemaVersion != CurrentSchemaVersion)
            {
                throw new InvalidDataException("Study library file schema version is not supported.");
            }

            return document.ToSnapshot();
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("Study library file is not valid JSON.", exception);
        }
    }

    public void Save(StudyLibrarySnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        string? directory = Path.GetDirectoryName(_storageFilePath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new InvalidOperationException("Storage file path must include a directory.");
        }

        Directory.CreateDirectory(directory);

        string temporaryFilePath = Path.Combine(
            directory,
            $".{Path.GetFileName(_storageFilePath)}.{Guid.NewGuid():N}.tmp");

        try
        {
            LibraryDocument document = LibraryDocument.FromSnapshot(snapshot);
            string json = JsonSerializer.Serialize(document, JsonOptions);
            File.WriteAllText(temporaryFilePath, json);

            if (File.Exists(_storageFilePath))
            {
                File.Replace(temporaryFilePath, _storageFilePath, destinationBackupFileName: null);
                return;
            }

            File.Move(temporaryFilePath, _storageFilePath);
        }
        finally
        {
            if (File.Exists(temporaryFilePath))
            {
                File.Delete(temporaryFilePath);
            }
        }
    }

    private sealed class LibraryDocument
    {
        public int SchemaVersion { get; set; } = CurrentSchemaVersion;

        public List<CourseDocument> Courses { get; set; } = [];

        public List<LessonProgressDocument> Progress { get; set; } = [];

        public StudyPreferencesDocument Preferences { get; set; } = StudyPreferencesDocument.Default;

        public static LibraryDocument FromSnapshot(StudyLibrarySnapshot snapshot)
        {
            return new LibraryDocument
            {
                Courses = snapshot.Courses.Select(CourseDocument.FromEntry).ToList(),
                Progress = snapshot.Progress.Select(LessonProgressDocument.FromEntry).ToList(),
                Preferences = StudyPreferencesDocument.FromPreferences(snapshot.Preferences)
            };
        }

        public StudyLibrarySnapshot ToSnapshot()
        {
            return new StudyLibrarySnapshot(
                Courses.Select(course => course.ToEntry()),
                Progress.Select(progress => progress.ToEntry()),
                Preferences.ToPreferences());
        }
    }

    private sealed class CourseDocument
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string RootPath { get; set; } = string.Empty;

        public List<CourseItemDocument> Items { get; set; } = [];

        public DateTimeOffset ImportedAt { get; set; }

        public static CourseDocument FromEntry(CourseCatalogEntry entry)
        {
            return new CourseDocument
            {
                Id = entry.Id,
                Title = entry.Title,
                RootPath = entry.RootPath,
                Items = entry.Items.Select(CourseItemDocument.FromItem).ToList(),
                ImportedAt = entry.ImportedAt
            };
        }

        public CourseCatalogEntry ToEntry()
        {
            return new CourseCatalogEntry(
                Id,
                Title,
                RootPath,
                Items.Select(item => item.ToItem()),
                ImportedAt);
        }
    }

    private sealed class CourseItemDocument
    {
        public CourseCatalogItemType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? RelativePath { get; set; }

        public List<CourseItemDocument> Children { get; set; } = [];

        public static CourseItemDocument FromItem(CourseCatalogItem item)
        {
            return new CourseItemDocument
            {
                Type = item.Type,
                Title = item.Title,
                RelativePath = item.RelativePath,
                Children = item.Children.Select(FromItem).ToList()
            };
        }

        public CourseCatalogItem ToItem()
        {
            return new CourseCatalogItem(
                Type,
                Title,
                RelativePath,
                Children.Select(child => child.ToItem()));
        }
    }

    private sealed class LessonProgressDocument
    {
        public Guid LessonId { get; set; }

        public TimeSpan WatchedDuration { get; set; }

        public bool IsCompleted { get; set; }

        public static LessonProgressDocument FromEntry(LessonProgressEntry entry)
        {
            return new LessonProgressDocument
            {
                LessonId = entry.LessonId,
                WatchedDuration = entry.WatchedDuration,
                IsCompleted = entry.IsCompleted
            };
        }

        public LessonProgressEntry ToEntry()
        {
            return new LessonProgressEntry(LessonId, WatchedDuration, IsCompleted);
        }
    }

    private sealed class StudyPreferencesDocument
    {
        public decimal DefaultPlaybackSpeed { get; set; } = 1.0m;

        public bool IntroSkipEnabled { get; set; }

        public TimeSpan IntroSkipDuration { get; set; }

        public static StudyPreferencesDocument Default { get; } = FromPreferences(StudyPreferences.Default);

        public static StudyPreferencesDocument FromPreferences(StudyPreferences preferences)
        {
            return new StudyPreferencesDocument
            {
                DefaultPlaybackSpeed = preferences.DefaultPlaybackSpeed,
                IntroSkipEnabled = preferences.IntroSkipEnabled,
                IntroSkipDuration = preferences.IntroSkipDuration
            };
        }

        public StudyPreferences ToPreferences()
        {
            return new StudyPreferences(DefaultPlaybackSpeed, IntroSkipEnabled, IntroSkipDuration);
        }
    }
}
