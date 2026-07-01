using FluentAssertions;
using School.Exams.Application.Tests.Helpers;
using School.Exams.Application.Validators;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using School.Exams.Infrastructure.Persistence;
using Xunit;

namespace School.Exams.Application.Tests.Validators;

public class RegisterExamValidatorTests
{
    private static RegisterExamRequest Valid() =>
        new("MAT", 12345, new DateOnly(2026, 5, 20), 5);

    private static async Task SeedAsync(SchoolDbContext db)
    {
        db.Subjects.Add(new Subject
        {
            SubjectCode = "MAT",
            Name = "Математика",
            Grade = 7,
            TeacherFirstName = "Иван",
            TeacherLastName = "Петров"
        });
        db.Students.Add(new Student
        {
            StudentNumber = 12345,
            FirstName = "Пётр",
            LastName = "Сидоров",
            Grade = 7
        });
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task Valid_request_passes()
    {
        await using var db = SchoolDbContextFactory.Create();
        await SeedAsync(db);
        var validator = new RegisterExamValidator(db);

        var result = await validator.ValidateAsync(Valid());

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Missing_Subject_fails()
    {
        await using var db = SchoolDbContextFactory.Create();
        await SeedAsync(db);
        var validator = new RegisterExamValidator(db);

        var result = await validator.ValidateAsync(Valid() with { SubjectCode = "ZZZ" });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(RegisterExamRequest.SubjectCode) &&
            e.ErrorMessage.Contains("does not exist"));
    }

    [Fact]
    public async Task Missing_Student_fails()
    {
        await using var db = SchoolDbContextFactory.Create();
        await SeedAsync(db);
        var validator = new RegisterExamValidator(db);

        var result = await validator.ValidateAsync(Valid() with { StudentNumber = 99999 });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(RegisterExamRequest.StudentNumber) &&
            e.ErrorMessage.Contains("does not exist"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("ABCD")]
    public async Task SubjectCode_must_be_exactly_3_chars(string code)
    {
        await using var db = SchoolDbContextFactory.Create();
        await SeedAsync(db);
        var validator = new RegisterExamValidator(db);

        var result = await validator.ValidateAsync(Valid() with { SubjectCode = code });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterExamRequest.SubjectCode));
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)6)]
    public async Task Score_out_of_range_fails(byte score)
    {
        await using var db = SchoolDbContextFactory.Create();
        await SeedAsync(db);
        var validator = new RegisterExamValidator(db);

        var result = await validator.ValidateAsync(Valid() with { Score = score });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterExamRequest.Score));
    }
}
