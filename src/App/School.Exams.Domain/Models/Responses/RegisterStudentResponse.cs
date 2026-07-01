namespace School.Exams.Domain.Models.Responses;

public sealed record RegisterStudentResponse(
    int StudentNumber,
    string FirstName,
    string LastName,
    byte Grade);
