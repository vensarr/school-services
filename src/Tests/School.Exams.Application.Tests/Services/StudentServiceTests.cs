using FluentAssertions;
using School.Exams.Application.Services;
using School.Exams.Application.Tests.Helpers;
using School.Exams.Domain.Entities;
using School.Exams.Domain.Models.Requests;
using Xunit;

namespace School.Exams.Application.Tests.Services;

public class StudentServiceTests
{
    private static RegisterStudentRequest ValidRequest() =>
        new(12345, "Иван", "Иванов", 7);

    [Fact]
    public async Task RegisterAsync_persists_student_and_returns_echo()
    {
        await using var db = SchoolDbContextFactory.Create();
        var service = new StudentService(db);

        var response = await service.RegisterAsync(ValidRequest(), CancellationToken.None);

        response.StudentNumber.Should().Be(12345);
        response.FirstName.Should().Be("Иван");
        response.LastName.Should().Be("Иванов");
        response.Grade.Should().Be(7);

        db.Students.Should().ContainSingle(x => x.StudentNumber == 12345);
    }

    [Fact]
    public async Task GetAllAsync_returns_all_stored_students()
    {
        await using var db = SchoolDbContextFactory.Create();
        db.Students.AddRange(
            new Student { StudentNumber = 1, FirstName = "A", LastName = "B", Grade = 5 },
            new Student { StudentNumber = 2, FirstName = "C", LastName = "D", Grade = 6 });
        await db.SaveChangesAsync();

        var service = new StudentService(db);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(x => x.StudentNumber).Should().BeEquivalentTo(new[] { 1, 2 });
    }

    [Fact]
    public async Task GetAllAsync_returns_empty_when_no_students()
    {
        await using var db = SchoolDbContextFactory.Create();
        var service = new StudentService(db);

        var result = await service.GetAllAsync(CancellationToken.None);

        result.Should().BeEmpty();
    }
}
