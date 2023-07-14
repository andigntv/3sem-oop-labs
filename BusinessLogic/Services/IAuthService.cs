using BusinessLogic.Dto;
using DataAccess.Entities;

namespace BusinessLogic.Services;

public interface IAuthService
{
    Task<EmployeeDto> LogInAsync(string login, string password, CancellationToken cancellationToken);
    void LogOutAsync(CancellationToken cancellationToken);
    Task<EmployeeDto> GetCurEmployee(CancellationToken cancellationToken);
}