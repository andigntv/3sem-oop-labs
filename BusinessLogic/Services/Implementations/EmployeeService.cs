using BusinessLogic.Dto;
using BusinessLogic.Exceptions;
using BusinessLogic.Extensions;
using BusinessLogic.Mapping;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models;

namespace BusinessLogic.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly DatabaseContext _context;

    public EmployeeService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<EmployeeDto> CreateEmployeeAsync(string name, Guid? chiefId, AccessLevel accessLevel, string login, string password , CancellationToken cancellationToken)
    {
        if (_context.Employees.ToList().Find(x => x.Login!.Equals(login)) is not null)
            throw new LoginException("User with this login already exists");
        
        Employee chief = await _context.Employees.GetEntityAsync(chiefId, cancellationToken);
        var employee = new Employee(name, Guid.NewGuid(), chief, accessLevel, login, password);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return employee.AsDto();
    }

    public async Task<EmployeeDto> AddSubordinateAsync(Guid chiefId, Guid subordinateId, CancellationToken cancellationToken)
    {
        Employee chief = await _context.Employees.GetEntityAsync(chiefId, cancellationToken);
        Employee subordinate = await _context.Employees.GetEntityAsync(subordinateId, cancellationToken);
        
        chief.Subordinates!.Add(subordinate);
        subordinate.Chief = chief;
        await _context.SaveChangesAsync(cancellationToken);

        return subordinate.AsDto();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.GetEntityAsync(id, cancellationToken);
        return employee.AsDto();
    }
}