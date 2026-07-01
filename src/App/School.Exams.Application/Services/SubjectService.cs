using Microsoft.EntityFrameworkCore;
using School.Exams.Application.Abstractions;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Services;

public sealed class SubjectService : ISubjectService
{
    private readonly SchoolDbContext _dbContext;

    public SubjectService(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RegisterSubjectResponse> RegisterAsync(RegisterSubjectRequest request, CancellationToken cancellationToken)
    {
        var subject = new Subject
        {
            SubjectCode = request.SubjectCode,
            Name = request.Name,
            Grade = request.Grade,
            TeacherFirstName = request.TeacherFirstName,
            TeacherLastName = request.TeacherLastName
        };

        _dbContext.Subjects.Add(subject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterSubjectResponse(
            subject.SubjectCode,
            subject.Name,
            subject.Grade,
            subject.TeacherFirstName,
            subject.TeacherLastName);
    }

    public async Task<IReadOnlyList<RegisterSubjectResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Subjects
            .AsNoTracking()
            .Select(x => new RegisterSubjectResponse(
                x.SubjectCode,
                x.Name,
                x.Grade,
                x.TeacherFirstName,
                x.TeacherLastName))
            .ToListAsync(cancellationToken);
    }
}
