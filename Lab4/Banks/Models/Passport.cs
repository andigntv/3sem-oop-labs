using Banks.Exceptions;

namespace Banks.Models;

public class Passport : IEquatable<Passport>
{
    private const int SeriesLength = 4;
    private const int NumberLength = 6;
    public Passport(string series, string number)
    {
        if (string.IsNullOrWhiteSpace(series))
            throw new ArgumentException("Passport series cannot be empty");
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Passport number cannot be empty");

        if (series.Length != SeriesLength)
            throw new PassportException("Invalid lenght of passport series");
        if (number.Length != NumberLength)
            throw new PassportException("Invalid lenght of passport number");

        if (!uint.TryParse(series, out uint tempSeries))
            throw new PassportException("Invalid format of passport series");
        if (!uint.TryParse(number, out uint tempNumber))
            throw new PassportException("Invalid format of passport number");

        (Series, Number) = (tempSeries, tempNumber);
    }

    public uint Series { get; }
    public uint Number { get; }

    public bool Equals(Passport? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Series == other.Series && Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Passport)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Series, Number);
    }
}