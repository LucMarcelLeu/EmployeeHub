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
            .AsNoTracking()
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                DepartmentId = x.DepartmentId,
                Department = x.Department != null ? x.Department.Name : null
            })
            .ToListAsync();
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(x => x.Department)
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                DepartmentId = x.DepartmentId,
                Department = x.Department != null ? x.Department.Name : null
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
            DepartmentId = dto.DepartmentId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(employee.Id))!;
    }

    public async Task<EmployeeDto?> UpdateAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            return null;

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.DepartmentId = dto.DepartmentId;

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

    private static EmployeeDto Map(Employee x)
    {
        return new EmployeeDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Department = x.Department != null ? x.Department.Name : null
        };
    }
}