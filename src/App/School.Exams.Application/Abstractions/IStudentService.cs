using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;

namespace School.Exams.Application.Abstractions;

public interface IStudentService
{
    Task<RegisterStudentResponse> RegisterAsync(RegisterStudentRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<RegisterStudentResponse>> GetAllAsync(CancellationToken cancellationToken);
}
