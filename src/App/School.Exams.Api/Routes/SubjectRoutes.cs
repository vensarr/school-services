namespace School.Exams.Api.Routes;

public static class SubjectRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    internal static class Subject
    {
        internal const string Main = Base + "/subject";
        internal const string Index = Base + "/subjects";
    }
}
