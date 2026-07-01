namespace School.Exams.Api.Routes;

public static class StudentRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    internal static class Student
    {
        internal const string Main = Base + "/student";
        internal const string Index = Base + "/students";
    }
}
