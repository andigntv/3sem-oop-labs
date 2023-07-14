using BusinessLogic.Dto;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("/Employee/Create")]
    public async Task<ActionResult<EmployeeDto>> CreateAsync([FromBody] CreateEmployeeModel model)
    {
        EmployeeDto employee = await _service.CreateEmployeeAsync(model.Name, null, model.AccessLevel, model.Login,
            model.Password, CancellationToken);
        return Ok(employee);
    }

    [HttpPost("/Subordinate/Add")]
    public async Task<ActionResult<EmployeeDto>> AddSubordinateAsync([FromBody] SubordinateAddingModel model)
    {
        EmployeeDto employeeDto =
            await _service.AddSubordinateAsync(model.ChiefId, model.SubordinateId, CancellationToken);
        return Ok(employeeDto);
    }

    [HttpGet("/Employee/Get")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeAsync(Guid id)
    {
        EmployeeDto employeeDto = await _service.GetEmployeeAsync(id, CancellationToken);
        return Ok(employeeDto);
    }
}