using BusinessLogic.Dto;
using DataAccess.Entities;

namespace BusinessLogic.Mapping;

public static class EmployeeMapping
{
    public static EmployeeDto AsDto(this Employee employee)
        => new EmployeeDto(
            employee.Id,
            employee.AccessLevel,
            employee.Name,
            employee.Chief?.Id,
            employee.Subordinates!.Select(x => x.Id).ToList(),
            employee.Sources.Select(x => x.AsDto()).ToList(),
            employee.Login!,
            employee.Password!);
}