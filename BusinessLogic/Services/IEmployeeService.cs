using BusinessLogic.Dto;
using DataAccess.Entities;
using DataAccess.Models;

namespace BusinessLogic.Services;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(string name, Guid? chiefId, AccessLevel accessLevel, string login, string password,
        CancellationToken cancellationToken);

    Task<EmployeeDto> AddSubordinateAsync(Guid chiefId, Guid subordinateId, CancellationToken cancellationToken);
    Task<EmployeeDto> GetEmployeeAsync(Guid id, CancellationToken cancellationToken);
}