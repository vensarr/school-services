namespace School.Exams.Domain.Models.Responses;

public sealed record RegisterSubjectResponse(
    string SubjectCode,
    string Name,
    byte Grade,
    string TeacherFirstName,
    string TeacherLastName);
