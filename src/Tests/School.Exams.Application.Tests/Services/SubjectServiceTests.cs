using FluentAssertions;
using School.Exams.Application.Services;
using School.Exams.Application.Tests.Helpers;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using Xunit;

namespace School.Exams.Application.Tests.Services;

public class SubjectServiceTests
{
    private static RegisterSubjectRequest ValidRequest() =>
        new("MAT", "Математика", 7, "Иван", "Петров");

    [Fact]
    public async Task RegisterAsync_persists_subject_and_returns_echo()
    {
        await using var db = SchoolDbContextFactory.Create();
        var service = new SubjectService(db);

        var response = await service.RegisterAsync(ValidRequest(), CancellationToken.None);

        response.SubjectCode.Should().Be("MAT");
        response.Name.Should().Be("Математика");
        response.Grade.Should().Be(7);
        response.TeacherFirstName.Should().Be("Иван");
        response.TeacherLastName.Should().Be("Петров");

        db.Subjects.Should().ContainSingle(x => x.SubjectCode == "MAT");
    }

    [Fact]
    public async Task GetAllAsync_returns_all_stored_subjects()
    {
        await using var db = SchoolDbContextFactory.Create();
        db.Subjects.AddRange(
            new Subject { SubjectCode = "MAT", Name = "Math", Grade = 7, TeacherFirstName = "A", TeacherLastName = "B" },
            new Subject { SubjectCode = "PHY", Name = "Physics", Grade = 8, TeacherFirstName = "C", TeacherLastName = "D" });
        await db.SaveChangesAsync();

        var service = new SubjectService(db);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(x => x.SubjectCode).Should().BeEquivalentTo(new[] { "MAT", "PHY" });
    }

    [Fact]
    public async Task GetAllAsync_returns_empty_when_no_subjects()
    {
        await using var db = SchoolDbContextFactory.Create();
        var service = new SubjectService(db);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().BeEmpty();
    }
}
