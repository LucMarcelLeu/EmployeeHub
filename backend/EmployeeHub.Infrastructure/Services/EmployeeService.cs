using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Application.Employees.Interfaces;
using EmployeeHub.Domain.Entities;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHub.Infrastructure.Services;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeHubDbContext _context;
    public EmployeeService(EmployeeHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(string? search = null)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.FirstName.Contains(search) ||
                e.LastName.Contains(search) ||
                e.Email.Contains(search));
        }

        return await query
            .Include(x => x.Department)
            .Include(x => x.Skills)
                .ThenInclude(x => x.Skill)
            .AsNoTracking()
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                EntryDate = x.EntryDate,
                ExitDate = x.ExitDate,
                DepartmentId = x.DepartmentId,
                Department = x.Department != null ? x.Department.Name : null,
                Skills = x.Skills
                    .Select(s => new EmployeeSkillDto
                    {
                        EmployeeId = s.EmployeeId,
                        SkillId = s.SkillId,
                        SkillName = s.Skill.Name
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(x => x.Department)
            .Include(x => x.Skills)
                .ThenInclude(x => x.Skill)
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                EntryDate = x.EntryDate,
                ExitDate = x.ExitDate,
                DepartmentId = x.DepartmentId,
                Department = x.Department != null ? x.Department.Name : null,
                Skills = x.Skills
                    .Select(s => new EmployeeSkillDto
                    {
                        EmployeeId = s.EmployeeId,
                        SkillId = s.SkillId,
                        SkillName = s.Skill.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            EntryDate = dto.EntryDate,
            ExitDate = dto.ExitDate,
            DepartmentId = dto.DepartmentId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        if (dto.SkillIds.Count > 0)
        {
            var skillIds = dto.SkillIds.Distinct().ToList();

            employee.Skills = skillIds
                .Select(skillId => new EmployeeSkill
                {
                    EmployeeId = employee.Id,
                    SkillId = skillId
                })
                .ToList();

            await _context.SaveChangesAsync();
        }

        return (await GetByIdAsync(employee.Id))!;
    }

    public async Task<EmployeeDto?> UpdateAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees
            .Include(x => x.Skills)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
            return null;

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.EntryDate = dto.EntryDate;
        employee.ExitDate = dto.ExitDate;
        employee.DepartmentId = dto.DepartmentId;

        var skillIds = dto.SkillIds.Distinct().ToList();

        employee.Skills.Clear();

        foreach (var skillId in skillIds)
        {
            employee.Skills.Add(new EmployeeSkill
            {
                EmployeeId = employee.Id,
                SkillId = skillId
            });
        }

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return true;
    }
}