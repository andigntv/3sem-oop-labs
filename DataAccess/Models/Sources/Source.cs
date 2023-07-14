using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Models.Sources;

public class Source
{
    public Source(Guid id, Employee owner, string type, string info)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException(type);
        if (string.IsNullOrWhiteSpace(info))
            throw new ArgumentNullException(info);
        ArgumentNullException.ThrowIfNull(owner);

        Type = type;
        Info = info;
        Id = id;
        Owner = owner;
    }
    protected Source() {}
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public string? Info { get; set; }
    public virtual Employee? Owner { get; set; }
}