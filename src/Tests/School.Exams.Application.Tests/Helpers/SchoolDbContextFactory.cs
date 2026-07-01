using Microsoft.EntityFrameworkCore;
using School.Exams.Infrastructure.Persistence;

namespace School.Exams.Application.Tests.Helpers;

internal static class SchoolDbContextFactory
{
    public static SchoolDbContext Create()
    {
        var options = new DbContextOptionsBuilder<SchoolDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SchoolDbContext(options);
    }
}
