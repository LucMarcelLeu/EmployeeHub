using EmployeeHub.Api.Exceptions;
using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Application.Employees.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHub.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;
    private readonly IOllamaChatService _ollamaChatService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeService service,
        IOllamaChatService ollamaChatService,
        ILogger<EmployeesController> logger)
    {
        _service = service;
        _ollamaChatService = ollamaChatService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
        => Ok(await _service.GetAllAsync(search));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _service.GetByIdAsync(id);
        if (employee == null)
        {
            throw new NotFoundException($"Employee '{id}' was not found.");
        }

        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        _logger.LogInformation(
            "Creating employee {Email}",
            dto.Email);

        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(Guid id, UpdateEmployeeDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id}/ai-summary")]
    [Authorize]
    public async Task<IActionResult> AiSummary(Guid id, [FromBody] AiChatRequestDto request)
    {
        var employee = await _service.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        var prompt = string.IsNullOrWhiteSpace(request.Prompt)
            ? "Erstelle eine kompakte Zusammenfassung dieses Mitarbeiters für ein HR-Dashboard."
            : request.Prompt;

        var answer = await _ollamaChatService.AskAsync(prompt, employee);

        return Ok(new AiChatResponseDto
        {
            Answer = answer,
            Summary = answer,
            Name = $"{employee.FirstName} {employee.LastName}".Trim(),
            Department = employee.Department ?? "Unbekannt",
            Skills = employee.Skills?.Count > 0
                ? string.Join(", ", employee.Skills.Select(x => x.SkillName ?? "Unbekannt"))
                : "Keine Skills hinterlegt",
            Email = employee.Email
        });
    }
}