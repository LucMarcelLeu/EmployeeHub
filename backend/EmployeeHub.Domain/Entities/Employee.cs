namespace EmployeeHub.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateOnly? EntryDate { get; set; }

    public DateOnly? ExitDate { get; set; }

    public Guid? DepartmentId { get; set; }

    public Department? Department { get; set; }

    public ICollection<EmployeeSkill> Skills { get; set; }
        = new List<EmployeeSkill>();
}