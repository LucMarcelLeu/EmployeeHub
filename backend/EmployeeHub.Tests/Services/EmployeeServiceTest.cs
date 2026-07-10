using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Infrastructure.Services;
using EmployeeHub.Tests.Helpers;
using FluentAssertions;

namespace EmployeeHub.Tests.Services;

public class EmployeeServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var employeeId = Guid.NewGuid();

        context.Employees.Add(new Domain.Entities.Employee
        {
            Id = employeeId,
            FirstName = "Max",
            LastName = "Nice",
            Email = "max@test.ch"
        });

        await context.SaveChangesAsync();

        var service = new EmployeeService(context);

        // Act
        var result = await service.GetByIdAsync(employeeId);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("Max");
        result.LastName.Should().Be("Nice");
        result.Email.Should().Be("max@test.ch");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();
        var service = new EmployeeService(context);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateEmployee()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();
        var service = new EmployeeService(context);
        var dto = new CreateEmployeeDto
        {
            FirstName = "Anna",
            LastName = "Muster",
            Email = "anna@test.ch"
        };

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();

        result.Id.Should().NotBe(Guid.Empty);
        result.FirstName.Should().Be("Anna");
        result.LastName.Should().Be("Muster");
        result.Email.Should().Be("anna@test.ch");

        // Prüfen, ob wirklich gespeichert wurde
        var employee = await context.Employees.FindAsync(result.Id);

        employee.Should().NotBeNull();
        employee!.FirstName.Should().Be("Anna");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEmployees()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        context.Employees.AddRange(
            new Domain.Entities.Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Nice",
                Email = "max@test.ch"
            },
            new Domain.Entities.Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Anna",
                LastName = "Muster",
                Email = "anna@test.ch"
            });

        await context.SaveChangesAsync();
        var service = new EmployeeService(context);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);

        result.Should()
            .Contain(x => x.FirstName == "Max");

        result.Should()
            .Contain(x => x.FirstName == "Anna");
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterBySearch()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        context.Employees.AddRange(
            new Domain.Entities.Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Nice",
                Email = "max@test.ch"
            },
            new Domain.Entities.Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Anna",
                LastName = "Muster",
                Email = "anna@test.ch"
            });

        await context.SaveChangesAsync();
        var service = new EmployeeService(context);

        // Act
        var result = await service.GetAllAsync("Max");

        // Assert
        result.Should().HaveCount(1);
        result.First().FirstName
            .Should()
            .Be("Max");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployee_WhenEmployeeExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var id = Guid.NewGuid();
        context.Employees.Add(new Domain.Entities.Employee
        {
            Id = id,
            FirstName = "Max",
            LastName = "Old",
            Email = "old@test.ch"
        });

        await context.SaveChangesAsync();

        var service = new EmployeeService(context);

        var dto = new UpdateEmployeeDto
        {
            FirstName = "Max",
            LastName = "Updated",
            Email = "updated@test.ch"
        };

        // Act
        var result = await service.UpdateAsync(id, dto);

        // Assert
        result.Should().NotBeNull();

        result!.LastName
            .Should()
            .Be("Updated");

        result.Email
            .Should()
            .Be("updated@test.ch");

        // Prüfen, ob DB wirklich geändert wurde
        var employee = await context.Employees.FindAsync(id);

        employee.Should().NotBeNull();

        employee!.LastName
            .Should()
            .Be("Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEmployee_WhenEmployeeExists()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var id = Guid.NewGuid();

        context.Employees.Add(new Domain.Entities.Employee
        {
            Id = id,
            FirstName = "Max",
            LastName = "Delete",
            Email = "delete@test.ch"
        });

        await context.SaveChangesAsync();

        var service = new EmployeeService(context);

        // Act
        var result = await service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();

        var employee = await context.Employees.FindAsync(id);

        employee.Should().BeNull();
    }


    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenEmployeeDoesNotExist()
    {
        // Arrange
        await using var context = TestDbContextFactory.Create();

        var service = new EmployeeService(context);

        // Act
        var result = await service.DeleteAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }
}