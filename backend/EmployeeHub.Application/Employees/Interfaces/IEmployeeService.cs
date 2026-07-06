using EmployeeHub.Application.Employees.DTOs;

namespace EmployeeHub.Application.Employees.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync(string? search = null);

    Task<EmployeeDto?> GetByIdAsync(Guid id);


    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);

    Task<EmployeeDto?> UpdateAsync(Guid id, UpdateEmployeeDto dto);

    Task<bool> DeleteAsync(Guid id);
}