using Isu.Exceptions;

namespace Isu.Models;

public class GroupName : IEquatable<GroupName>
{
    private const int CorrectIdentifierLength = 6;

    public GroupName(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier is empty");

        if (identifier.Length is not CorrectIdentifierLength)
            throw new IsuException("Invalid identifier length");

        Identifier = identifier;
    }

    public string Identifier { get; }

    public static bool operator ==(GroupName? left, GroupName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(GroupName? left, GroupName? right)
    {
        return !Equals(left, right);
    }

    public bool Equals(GroupName? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Identifier == other.Identifier;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((GroupName)obj);
    }

    public override int GetHashCode()
    {
        return Identifier.GetHashCode();
    }
}