using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Elective : IEquatable<Elective>
{
    private List<ElectiveGroup> _groups;

    public Elective(string name, Faculty faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of Electives cannot be empty");
        Name = name;
        Faculty = faculty;
        _groups = new List<ElectiveGroup>();
    }

    public Faculty Faculty { get; }
    public string Name { get; }
    public IReadOnlyList<ElectiveGroup> Groups => _groups;

    public ElectiveGroup? FindGroup(ElectiveGroup group)
    {
        if (group is null)
            throw new ArgumentException("Group cannot be null");
        return _groups.Find(x => x.Equals(group));
    }

    public ElectiveGroup AddGroup(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of group cannot be empty");
        var group = new ElectiveGroup(name, Faculty);
        if (_groups.Contains(group))
            throw new NameException("Group with this name already exists in this elective");
        _groups.Add(group);
        return group;
    }

    public bool Equals(Elective? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Faculty == other.Faculty && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Elective)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Faculty, Name);
    }
}