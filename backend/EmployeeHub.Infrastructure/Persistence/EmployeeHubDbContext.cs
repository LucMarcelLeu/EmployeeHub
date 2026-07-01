using EmployeeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHub.Infrastructure.Persistence;

public class EmployeeHubDbContext : DbContext
{
    public EmployeeHubDbContext(
        DbContextOptions<EmployeeHubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<EmployeeSkill>()
            .HasKey(x => new
            {
                x.EmployeeId,
                x.SkillId
            });
    }
}