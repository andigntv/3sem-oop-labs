using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.Sources;

namespace DataAccess.Entities;

public class Employee
{
    public Employee(string name, Guid id, Employee? chief, AccessLevel accessLevel, string login, string password)
    {
        Name = name;
        Id = id;
        Chief = chief;
        AccessLevel = accessLevel;
        Login = login;
        Password = password;
        Sources = new List<Source>();
        Subordinates = new List<Employee>();
    }

    protected Employee() { }
    public virtual AccessLevel AccessLevel { get; set; }
    public string Name { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public Guid Id { get; set; }
    public virtual Employee? Chief { get; set; }
    public virtual List<Source> Sources { get; set; }
    public virtual List<Employee>? Subordinates { get; set; }
}