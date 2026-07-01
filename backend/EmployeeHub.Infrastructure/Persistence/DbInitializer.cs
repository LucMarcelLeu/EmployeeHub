using EmployeeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHub.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        EmployeeHubDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Employees.AnyAsync())
            return;

        var backend =
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Backend Development"
            };

        var frontend =
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Frontend Development"
            };

        var csharp =
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "C#"
            };

        var angular =
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Angular"
            };

        var dotnet =
            new Skill
            {
                Id = Guid.NewGuid(),
                Name = ".NET"
            };

        var employee =
            new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Mustermann",
                Email = "max@example.com",
                Department = backend
            };

        context.Departments.AddRange(
            backend,
            frontend);

        context.Skills.AddRange(
            csharp,
            angular,
            dotnet);

        context.Employees.Add(employee);

        context.Entry(employee)
            .Collection(x => x.Skills)
            .Load();

        employee.Skills.Add(
            new EmployeeSkill
            {
                EmployeeId = employee.Id,
                SkillId = csharp.Id
            });

        employee.Skills.Add(
            new EmployeeSkill
            {
                EmployeeId = employee.Id,
                SkillId = dotnet.Id
            });

        await context.SaveChangesAsync();
    }
}