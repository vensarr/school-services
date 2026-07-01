using Microsoft.EntityFrameworkCore;
using School.Exams.Application.Abstractions;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Services;

public sealed class StudentService : IStudentService
{
    private readonly SchoolDbContext _dbContext;

    public StudentService(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RegisterStudentResponse> RegisterAsync(RegisterStudentRequest request, CancellationToken cancellationToken)
    {
        var student = new Student
        {
            StudentNumber = request.StudentNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Grade = request.Grade
        };

        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterStudentResponse(
            student.StudentNumber,
            student.FirstName,
            student.LastName,
            student.Grade);
    }

    public async Task<IReadOnlyList<RegisterStudentResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Students
            .AsNoTracking()
            .Select(x => new RegisterStudentResponse(
                x.StudentNumber,
                x.FirstName,
                x.LastName,
                x.Grade))
            .ToListAsync(cancellationToken);
    }
}
