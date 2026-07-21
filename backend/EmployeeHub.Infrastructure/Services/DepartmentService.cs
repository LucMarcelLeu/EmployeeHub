using EmployeeHub.Application.Departments.DTOs;
using EmployeeHub.Application.Departments.Interfaces;
using EmployeeHub.Domain.Entities;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeHub.Infrastructure.Services;

public class DepartmentService : IDepartmentService
{
    private readonly EmployeeHubDbContext _context;

    public DepartmentService(EmployeeHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        return await _context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                EmployeeCount = d.Employees.Count
            })
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        return await _context.Departments
            .Include(x => x.Employees)
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new DepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                EmployeeCount = x.Employees.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(department.Id))!;
    }

    public async Task<DepartmentDto?> UpdateAsync(Guid id, UpdateDepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return null;

        department.Name = dto.Name;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}