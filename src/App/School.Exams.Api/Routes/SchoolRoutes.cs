namespace School.Exams.Api.Routes;

public static class SchoolRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    internal static class Exam
    {
        internal const string Main = Base + "/exam";
        internal const string Index = Base + "/exams";
    }
    
    internal static class Student
    {
        internal const string Main = Base + "/student";
        internal const string Index = Base + "/students";
    }
    
    internal static class Subject
    {
        internal const string Main = Base + "/subject";
        internal const string Index = Base + "/subjects";
    }
}
