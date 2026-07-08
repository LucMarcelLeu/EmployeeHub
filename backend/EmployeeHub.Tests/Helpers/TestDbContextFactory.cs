using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHub.Tests.Helpers;

public static class TestDbContextFactory
{
    public static EmployeeHubDbContext Create()
    {
        var options = new DbContextOptionsBuilder<EmployeeHubDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new EmployeeHubDbContext(options);
    }
}