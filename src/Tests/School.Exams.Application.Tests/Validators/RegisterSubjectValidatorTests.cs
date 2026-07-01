using FluentAssertions;
using School.Exams.Application.Tests.Helpers;
using School.Exams.Application.Validators;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using Xunit;

namespace School.Exams.Application.Tests.Validators;

public class RegisterSubjectValidatorTests
{
    private static RegisterSubjectRequest Valid() =>
        new("MAT", "Математика", 7, "Иван", "Петров");

    [Fact]
    public async Task Valid_request_passes()
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterSubjectValidator(db);

        var result = await validator.ValidateAsync(Valid());

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData("ABCD")]
    public async Task SubjectCode_must_be_exactly_3_chars(string code)
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterSubjectValidator(db);

        var result = await validator.ValidateAsync(Valid() with { SubjectCode = code });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterSubjectRequest.SubjectCode));
    }

    [Fact]
    public async Task Name_cannot_exceed_30_chars()
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterSubjectValidator(db);

        var result = await validator.ValidateAsync(Valid() with { Name = new string('a', 31) });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterSubjectRequest.Name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(12)]
    public async Task Grade_out_of_range_fails(byte grade)
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterSubjectValidator(db);

        var result = await validator.ValidateAsync(Valid() with { Grade = grade });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterSubjectRequest.Grade));
    }

    [Fact]
    public async Task Teacher_names_cannot_exceed_20_chars()
    {
        await using var db = SchoolDbContextFactory.Create();
        var validator = new RegisterSubjectValidator(db);

        var result = await validator.ValidateAsync(Valid() with
        {
            TeacherFirstName = new string('a', 21),
            TeacherLastName = new string('b', 21)
        });

        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.PropertyName)
            .Should().Contain(new[]
            {
                nameof(RegisterSubjectRequest.TeacherFirstName),
                nameof(RegisterSubjectRequest.TeacherLastName)
            });
    }

    [Fact]
    public async Task Duplicate_SubjectCode_fails()
    {
        await using var db = SchoolDbContextFactory.Create();
        db.Subjects.Add(new Subject
        {
            SubjectCode = "MAT",
            Name = "Existing",
            Grade = 5,
            TeacherFirstName = "A",
            TeacherLastName = "B"
        });
        await db.SaveChangesAsync();

        var validator = new RegisterSubjectValidator(db);

        var result = await validator.ValidateAsync(Valid());

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == nameof(RegisterSubjectRequest.SubjectCode) &&
            e.ErrorMessage.Contains("already exists"));
    }
}
