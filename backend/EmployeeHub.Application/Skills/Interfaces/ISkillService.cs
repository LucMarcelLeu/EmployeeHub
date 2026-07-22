using EmployeeHub.Application.Skills.DTOs;

namespace EmployeeHub.Application.Skills.Interfaces;

public interface ISkillService
{
    Task<IEnumerable<SkillDto>> GetAllAsync();
}
