namespace EmployeeHub.Api.Models;

public sealed class ErrorResponse
{
    public int Status { get; init; }

    public string Message { get; init; } = string.Empty;

    public string? TraceId { get; init; }

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}