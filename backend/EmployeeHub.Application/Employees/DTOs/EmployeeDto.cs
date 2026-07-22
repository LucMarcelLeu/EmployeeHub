namespace EmployeeHub.Application.Employees.DTOs;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateOnly? EntryDate { get; set; }
    public DateOnly? ExitDate { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Department { get; set; }
    public List<EmployeeSkillDto> Skills { get; set; } = new();
}