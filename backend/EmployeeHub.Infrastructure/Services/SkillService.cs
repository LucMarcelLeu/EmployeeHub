using EmployeeHub.Application.Skills.DTOs;
using EmployeeHub.Application.Skills.Interfaces;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHub.Infrastructure.Services;

public class SkillService : ISkillService
{
    private readonly EmployeeHubDbContext _context;

    public SkillService(EmployeeHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SkillDto>> GetAllAsync()
    {
        return await _context.Skills
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SkillDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}
