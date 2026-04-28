using StudyLab.Domain.Courses;

namespace StudyLab.Domain.Tests.Courses;

public sealed class CourseStructureTests
{
    [Fact]
    public void CourseAllowsHierarchicalContentWithModulesTopicsAndLessons()
    {
        Course course = Course.Create("C# Completo");
        CourseModule module = CourseModule.Create("Modulo 1");
        Topic topic = Topic.Create("Fundamentos");
        Lesson lesson = Lesson.Create("Introducao", TimeSpan.FromMinutes(12));

        topic.AddLesson(lesson);
        module.AddTopic(topic);
        course.AddModule(module);

        Assert.Equal("C# Completo", course.Title);
        Assert.Single(course.Modules);
        Assert.Single(course.Modules[0].Topics);
        Assert.Single(course.Modules[0].Topics[0].Lessons);
    }

    [Fact]
    public void LessonRejectsEmptyTitle()
    {
        Assert.Throws<ArgumentException>(() =>
            Lesson.Create(" ", TimeSpan.FromMinutes(5)));
    }
}
