using FluentAssertions;
using School.Exams.Application.Services;
using School.Exams.Application.Tests.Helpers;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using Xunit;

namespace School.Exams.Application.Tests.Services;

public class ExamServiceTests
{
    private static RegisterExamRequest ValidRequest() =>
        new("MAT", 12345, new DateOnly(2026, 5, 20), 5);

    [Fact]
    public async Task RegisterAsync_persists_exam_and_returns_generated_id()
    {
        await using var db = SchoolDbContextFactory.Create();
        var service = new ExamService(db);

        var response = await service.RegisterAsync(ValidRequest(), CancellationToken.None);

        response.Id.Should().BeGreaterThan(0);
        response.SubjectCode.Should().Be("MAT");
        response.StudentNumber.Should().Be(12345);
        response.ExamDate.Should().Be(new DateOnly(2026, 5, 20));
        response.Score.Should().Be(5);

        db.Exams.Should().ContainSingle(x => x.Id == response.Id);
    }

    [Fact]
    public async Task GetAllAsync_returns_all_stored_exams()
    {
        await using var db = SchoolDbContextFactory.Create();
        db.Exams.AddRange(
            new Exam { SubjectCode = "MAT", StudentNumber = 1, ExamDate = new DateOnly(2026, 5, 20), Score = 5 },
            new Exam { SubjectCode = "PHY", StudentNumber = 2, ExamDate = new DateOnly(2026, 5, 21), Score = 4 });
        await db.SaveChangesAsync();

        var service = new ExamService(db);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(x => x.SubjectCode).Should().BeEquivalentTo(new[] { "MAT", "PHY" });
    }

    [Fact]
    public async Task GetAllAsync_returns_empty_when_no_exams()
    {
        await using var db = SchoolDbContextFactory.Create();
        var service = new ExamService(db);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().BeEmpty();
    }
}
