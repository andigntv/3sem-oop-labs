namespace Shops.Entities;

public class Product : IEquatable<Product>
{
    public Product(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product's name cannot be empty");
        Name = name;
    }

    public string Name { get; }

    public static bool operator ==(Product? left, Product? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Product? left, Product? right)
    {
        return !Equals(left, right);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((Product)obj);
    }

    public bool Equals(Product? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}