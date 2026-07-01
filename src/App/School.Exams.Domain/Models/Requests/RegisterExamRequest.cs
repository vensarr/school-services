namespace School.Exams.Domain.Models.Requests;

public sealed record RegisterExamRequest(
    string SubjectCode,
    int StudentNumber,
    DateOnly ExamDate,
    byte Score);
