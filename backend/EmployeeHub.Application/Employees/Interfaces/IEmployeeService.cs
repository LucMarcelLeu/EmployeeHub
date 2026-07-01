using EmployeeHub.Application.Employees.DTOs;

namespace EmployeeHub.Application.Employees.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();

    Task<EmployeeDto?> GetByIdAsync(Guid id);

    Task<EmployeeDto> CreateAsync(
    CreateEmployeeDto dto);

    Task DeleteAsync(Guid id);
}