using DataAccess.Models;

namespace Presentation.Models;

public record CreateEmployeeModel(string Name, AccessLevel AccessLevel, string Login, string Password);