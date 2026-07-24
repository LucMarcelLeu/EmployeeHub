using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Infrastructure.Services;
using FluentAssertions;

namespace EmployeeHub.Tests.Services;

public class OllamaChatServiceTests
{
    [Fact]
    public void BuildPrompt_ShouldIncludeEmployeeContext()
    {
        // Arrange
        var employee = new EmployeeDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Max",
            LastName = "Nice",
            Email = "max@test.ch",
            Department = "Engineering",
            Skills =
            [
                new EmployeeSkillDto { SkillName = "C#" },
                new EmployeeSkillDto { SkillName = ".NET" }
            ]
        };

        // Act
        var prompt = OllamaChatService.BuildPrompt("Summarize this employee", employee);

        // Assert
        prompt.Should().Contain("Max Nice");
        prompt.Should().Contain("Engineering");
        prompt.Should().Contain("C#");
        prompt.Should().Contain("Summarize this employee");
    }

    [Fact]
    public void BuildEmployeeSummary_ShouldReturnReadableGermanText()
    {
        // Arrange
        var employee = new EmployeeDto
        {
            FirstName = "Jane",
            LastName = "Fond",
            Email = "jane@test.ch",
            Department = "Crunch",
            Skills =
            [
                new EmployeeSkillDto { SkillName = "C#" },
                new EmployeeSkillDto { SkillName = "Angular" }
            ]
        };

        // Act
        var summary = OllamaChatService.BuildEmployeeSummary(employee);

        // Assert
        summary.Should().Contain("Jane Fond");
        summary.Should().Contain("Crunch");
        summary.Should().Contain("C#");
        summary.Should().Contain("Angular");
        summary.Should().Contain("HR-");
    }
}
