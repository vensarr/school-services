using FluentValidation;
using Microsoft.EntityFrameworkCore;
using School.Exams.Domain.Models.Requests;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Validators;

public sealed class RegisterSubjectValidator : AbstractValidator<RegisterSubjectRequest>
{
    private readonly SchoolDbContext _dbContext;

    public RegisterSubjectValidator(SchoolDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.SubjectCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(3)
            .MustAsync(BeUniqueCodeAsync)
                .WithMessage(x => $"Subject with code '{x.SubjectCode}' already exists.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Grade)
            .InclusiveBetween((byte)1, (byte)11);

        RuleFor(x => x.TeacherFirstName)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.TeacherLastName)
            .NotEmpty()
            .MaximumLength(20);
    }

    private Task<bool> BeUniqueCodeAsync(string code, CancellationToken cancellationToken)
        => _dbContext.Subjects
            .AsNoTracking()
            .AllAsync(x => x.SubjectCode != code, cancellationToken);
}
