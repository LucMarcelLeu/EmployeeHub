using EmployeeHub.Application.Employees.DTOs;

namespace EmployeeHub.Application.Employees.Interfaces;

public interface IOllamaChatService
{
    Task<string> AskAsync(string prompt, EmployeeDto? employee = null, CancellationToken cancellationToken = default);
}
