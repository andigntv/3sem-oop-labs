using DataAccess.Models;

namespace BusinessLogic.Dto;

public record EmployeeDto(
    Guid Id,
    AccessLevel AccessLevel,
    string Name, 
    Guid? ChiefId, 
    IReadOnlyCollection<Guid> SubordinatesId, 
    IReadOnlyCollection<SourceDto> Sources,
    string Login,
    string Password);