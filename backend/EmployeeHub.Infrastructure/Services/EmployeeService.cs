using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Application.Employees.Interfaces;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace EmployeeHub.Infrastructure.Employees;


public class EmployeeService : IEmployeeService
{
    private readonly EmployeeHubDbContext _context;


    public EmployeeService(EmployeeHubDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        return await _context.Employees
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Department = e.Department != null
                    ? e.Department.Name
                    : null
            })
            .ToListAsync();
    }


    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email
            })
            .FirstOrDefaultAsync();
    }
}