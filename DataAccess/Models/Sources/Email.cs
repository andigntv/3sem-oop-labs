using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Sources;
/**
public class Email : ISource
{
    public Email(Guid id, Employee owner, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        ArgumentNullException.ThrowIfNull(owner);

        Id = id;
        Owner = owner;
        Name = name;
    }

    public Guid Id { get; set; }
    protected Email() { }
    private string Name { get; set; }
    public virtual Employee Owner { get; set; }

    public string Info => $"Email: {Name}";
}**/