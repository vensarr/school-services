using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;

namespace School.Exams.Application.Abstractions;

public interface ISubjectService
{
    Task<RegisterSubjectResponse> RegisterAsync(RegisterSubjectRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<RegisterSubjectResponse>> GetAllAsync(CancellationToken cancellationToken);
}
