using EmployeeHub.Application.Employees.DTOs;
using EmployeeHub.Application.Employees.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(
        IEmployeeService service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(
            await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result =
            await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        var result =
            await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(Get),
            new { id = result.Id },
            result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}