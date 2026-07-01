namespace School.Exams.Domain.Models.Requests;

public sealed record RegisterStudentRequest(
    int StudentNumber,
    string FirstName,
    string LastName,
    byte Grade);
