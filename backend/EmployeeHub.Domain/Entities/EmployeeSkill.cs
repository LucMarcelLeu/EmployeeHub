namespace EmployeeHub.Domain.Entities;

public class EmployeeSkill
{
    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;


    public Guid SkillId { get; set; }

    public Skill Skill { get; set; } = null!;
}