using FluentValidation;
using Microsoft.EntityFrameworkCore;
using School.Exams.Domain.Models.Requests;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Validators;

public sealed class RegisterExamValidator : AbstractValidator<RegisterExamRequest>
{
    private readonly SchoolDbContext _dbContext;

    public RegisterExamValidator(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.SubjectCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(3)
            .MustAsync(SubjectExistsAsync)
                .WithMessage(x => $"Subject with code '{x.SubjectCode}' does not exist.");

        RuleFor(x => x.StudentNumber)
            .Cascade(CascadeMode.Stop)
            .InclusiveBetween(1, 99999)
            .MustAsync(StudentExistsAsync)
                .WithMessage(x => $"Student with number '{x.StudentNumber}' does not exist.");

        RuleFor(x => x.ExamDate)
            .NotEmpty();

        RuleFor(x => x.Score)
            .InclusiveBetween((byte)1, (byte)5);
    }

    private Task<bool> SubjectExistsAsync(string code, CancellationToken cancellationToken)
        => _dbContext.Subjects
            .AsNoTracking()
            .AnyAsync(x => x.SubjectCode == code, cancellationToken);

    private Task<bool> StudentExistsAsync(int number, CancellationToken cancellationToken)
        => _dbContext.Students
            .AsNoTracking()
            .AnyAsync(x => x.StudentNumber == number, cancellationToken);
}
