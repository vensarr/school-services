using FluentValidation;
using Microsoft.EntityFrameworkCore;
using School.Exams.Domain.Models.Requests;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Validators;

public sealed class RegisterStudentValidator : AbstractValidator<RegisterStudentRequest>
{
    private readonly SchoolDbContext _dbContext;

    public RegisterStudentValidator(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.StudentNumber)
            .Cascade(CascadeMode.Stop)
            .InclusiveBetween(1, 99999)
            .MustAsync(BeUniqueNumberAsync)
                .WithMessage(x => $"Student with number '{x.StudentNumber}' already exists.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Grade)
            .InclusiveBetween((byte)1, (byte)11);
    }

    private Task<bool> BeUniqueNumberAsync(int number, CancellationToken cancellationToken)
        => _dbContext.Students
            .AsNoTracking()
            .AllAsync(x => x.StudentNumber != number, cancellationToken);
}
