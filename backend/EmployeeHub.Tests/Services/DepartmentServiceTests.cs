using EmployeeHub.Domain.Entities;
using EmployeeHub.Infrastructure.Services;
using EmployeeHub.Tests.Helpers;
using FluentAssertions;

namespace EmployeeHub.Tests.Services;

public class DepartmentServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnDepartmentsOrderedByName()
    {
        // Arrange

        await using var context = TestDbContextFactory.Create();

        context.Departments.AddRange(
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "IT"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "HR"
            });

        await context.SaveChangesAsync();

        var service = new DepartmentService(context);

        // Act

        var result = (await service.GetAllAsync()).ToList();

        // Assert

        result.Should().HaveCount(3);

        result[0].Name.Should().Be("Finance");
        result[1].Name.Should().Be("HR");
        result[2].Name.Should().Be("IT");
    }
}