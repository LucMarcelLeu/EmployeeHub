namespace EmployeeHub.Application.Employees.DTOs;

public class EmployeeSkillDto
{
    public Guid EmployeeId { get; set; }

    public Guid SkillId { get; set; }

    public string? SkillName { get; set; }
}
