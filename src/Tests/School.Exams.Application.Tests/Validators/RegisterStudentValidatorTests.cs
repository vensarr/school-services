using FluentAssertions;
using School.Exams.Application.Tests.Helpers;
using School.Exams.Application.Validators;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using Xunit;

namespace School.Exams.Application.Tests.Validators;

public class RegisterStudentValidatorTests
{
    private static RegisterStudentRequest Valid() =>
        new(12345, "Иван", "Иванов", 7);

    [Fact]
    public async Task Valid_request_passes()
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterStudentValidator(db);

        var result = await validator.ValidateAsync(Valid());

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100000)]
    public async Task StudentNumber_out_of_range_fails(int number)
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterStudentValidator(db);

        var result = await validator.ValidateAsync(Valid() with { StudentNumber = number });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterStudentRequest.StudentNumber));
    }

    [Fact]
    public async Task Empty_FirstName_fails()
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterStudentValidator(db);

        var result = await validator.ValidateAsync(Valid() with { FirstName = "" });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterStudentRequest.FirstName));
    }

    [Fact]
    public async Task Names_cannot_exceed_30_chars()
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterStudentValidator(db);

        var result = await validator.ValidateAsync(Valid() with
        {
            FirstName = new string('a', 31),
            LastName = new string('b', 31)
        });

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.PropertyName)
            .Should().Contain(new[]
            {
                nameof(RegisterStudentRequest.FirstName),
                nameof(RegisterStudentRequest.LastName)
            });
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)12)]
    public async Task Grade_out_of_range_fails(byte grade)
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterStudentValidator(db);

        var result = await validator.ValidateAsync(Valid() with { Grade = grade });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterStudentRequest.Grade));
    }

    [Fact]
    public async Task Duplicate_StudentNumber_fails()
    {
        await using var db = SchoolDbContextFactory.Create();
        db.Students.Add(new Student
        {
            StudentNumber = 12345,
            FirstName = "Existing",
            LastName = "User",
            Grade = 5
        });
        await db.SaveChangesAsync();

        var validator = new RegisterStudentValidator(db);

        var result = await validator.ValidateAsync(Valid());

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(RegisterStudentRequest.StudentNumber) &&
            e.ErrorMessage.Contains("already exists"));
    }
}
