using EmployeeHub.Application.Departments.DTOs;
using EmployeeHub.Application.Departments.Interfaces;
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

    public Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<DepartmentDto?> UpdateAsync(Guid id, UpdateDepartmentDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}