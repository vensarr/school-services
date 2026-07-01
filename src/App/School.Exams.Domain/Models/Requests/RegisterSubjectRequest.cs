namespace School.Exams.Domain.Models.Requests;

public sealed record RegisterSubjectRequest(
    string SubjectCode,
    string Name,
    byte Grade,
    string TeacherFirstName,
    string TeacherLastName);
