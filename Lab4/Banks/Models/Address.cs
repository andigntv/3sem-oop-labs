using System.Text.RegularExpressions;
using Banks.Exceptions;

namespace Banks.Models;

public class Address
{
    public Address(string city, string street, string building)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Name of city cannot be empty");
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Name of street cannot be empty");
        if (string.IsNullOrWhiteSpace(building))
            throw new ArgumentException("Number of building cannot be empty");

        if (!Regex.IsMatch(city, @"^[a-zA-Z]+$"))
            throw new AddressException("Name of city must contain only letters");
        if (!Regex.IsMatch(street, @"^[a-zA-Z0-9]+$"))
            throw new AddressException("Name of street must contain only letters and digits");
        if (!Regex.IsMatch(building, @"^[0-9/k]+$"))
            throw new AddressException("Name of street must contain only digits, \"/\" or \"k\"");

        (City, Street, Building) = (city, street, building);
    }

    public string City { get; }
    public string Street { get; }
    public string Building { get; }
}