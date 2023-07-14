using BusinessLogic.Dto;
using BusinessLogic.Exceptions;
using BusinessLogic.Extensions;
using BusinessLogic.Mapping;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;

    public AuthService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> LogInAsync(string login, string password, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.FirstAsync(x => x.Login!.Equals(login) && x.Password!.Equals(password), cancellationToken: cancellationToken);
        _context.Logs.Add(new Log( employee.Id,Guid.NewGuid(), DateTime.Now, true));
        await _context.SaveChangesAsync(cancellationToken);
        return employee.AsDto();
    }

    public async void LogOutAsync(CancellationToken cancellationToken)
    {
        Log lastLog = _context.Logs.First();
        foreach (Log log in _context.Logs)
        {
            if (log.DateTime.CompareTo(lastLog.DateTime) > 0)
                lastLog = log;
        }
        _context.Logs.Add(new Log(lastLog.ClientId, Guid.NewGuid(), DateTime.Now, false));
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<EmployeeDto> GetCurEmployee(CancellationToken cancellationToken)
    {
        Log lastLog = _context.Logs.First();
        foreach (Log log in _context.Logs)
        {
            if (log.DateTime.CompareTo(lastLog.DateTime) > 0)
                lastLog = log;
        }
        if (!lastLog.Logged)
            throw new AuthException("No authorized employee");
        Employee employee = await _context.Employees.GetEntityAsync(lastLog.ClientId, cancellationToken);
        return employee.AsDto();
    }
}