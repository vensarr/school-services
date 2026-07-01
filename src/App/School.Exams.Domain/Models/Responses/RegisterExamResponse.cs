namespace School.Exams.Domain.Models.Responses;

public sealed record RegisterExamResponse(
    int Id,
    string SubjectCode,
    int StudentNumber,
    DateOnly ExamDate,
    byte Score);
