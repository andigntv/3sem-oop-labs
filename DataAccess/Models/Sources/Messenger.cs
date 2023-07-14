using DataAccess.Entities;
using DataAccess.Interfaces;
/**
namespace DataAccess.Models.Sources;

public class Messenger : ISource
{
    public Messenger(Guid id, Employee owner)
    {
        ArgumentNullException.ThrowIfNull(owner);
        Id = id;
        Owner = owner;
    }

    public Guid Id { get; set; }
    protected Messenger() { }
    public virtual Employee Owner { get; set; }
    public string Info => "Messenger";
}**/