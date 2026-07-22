using System.ComponentModel.DataAnnotations;

namespace EmployeeHub.Application.Employees.DTOs;

public class CreateEmployeeDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = "";

    public DateOnly? EntryDate { get; set; }

    public DateOnly? ExitDate { get; set; }

    public Guid? DepartmentId { get; set; }

    public List<Guid> SkillIds { get; set; } = new();
}