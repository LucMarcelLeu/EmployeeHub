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
    public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();

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

        builder.Entity<EmployeeSkill>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.Skills)
            .HasForeignKey(x => x.EmployeeId);

        builder.Entity<EmployeeSkill>()
            .HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(x => x.SkillId);
    }
}