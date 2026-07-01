using Microsoft.EntityFrameworkCore;
using School.Exams.Application.Abstractions;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Services;

public sealed class ExamService : IExamService
{
    private readonly SchoolDbContext _dbContext;

    public ExamService(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RegisterExamResponse> RegisterAsync(RegisterExamRequest request, CancellationToken cancellationToken)
    {
        var exam = new Exam
        {
            SubjectCode = request.SubjectCode,
            StudentNumber = request.StudentNumber,
            ExamDate = request.ExamDate,
            Score = request.Score
        };

        _dbContext.Exams.Add(exam);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterExamResponse(
            exam.Id,
            exam.SubjectCode,
            exam.StudentNumber,
            exam.ExamDate,
            exam.Score);
    }

    public async Task<IReadOnlyList<RegisterExamResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Exams
            .AsNoTracking()
            .Select(x => new RegisterExamResponse(
                x.Id,
                x.SubjectCode,
                x.StudentNumber,
                x.ExamDate,
                x.Score))
            .ToListAsync(cancellationToken);
    }
}
