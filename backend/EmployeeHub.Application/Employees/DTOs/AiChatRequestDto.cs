namespace EmployeeHub.Application.Employees.DTOs;

public class AiChatRequestDto
{
    public string Prompt { get; set; } = string.Empty;
    public Guid? EmployeeId { get; set; }
}
