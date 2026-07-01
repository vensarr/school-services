using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;

namespace School.Exams.Application.Abstractions;

public interface IExamService
{
    Task<RegisterExamResponse> RegisterAsync(RegisterExamRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<RegisterExamResponse>> GetAllAsync(CancellationToken cancellationToken);
}
