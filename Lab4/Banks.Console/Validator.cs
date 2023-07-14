using System.Text.RegularExpressions;
using Banks.Entities;
using Banks.Models;

namespace Banks.Console;

public static class Validator
{
    private const int SeriesLength = 4;
    private const int NumberLength = 6;
    public static bool PassportDataValidityCheck(string? series, string? number, Bank bank)
    {
        if (string.IsNullOrWhiteSpace(series))
            return false;
        if (string.IsNullOrWhiteSpace(number))
            return false;

        if (series.Length != SeriesLength)
            return false;
        if (number.Length != NumberLength)
            return false;

        if (!uint.TryParse(series, out uint tempSeries))
            return false;
        if (!uint.TryParse(number, out uint tempNumber))
            return false;

        return bank.Clients.All(client => client.Passport == null
                                          || !client.Passport.Series.Equals(tempSeries)
                                          || !client.Passport.Number.Equals(tempNumber));
    }

    public static bool AddressValidityCheck(string? city, string? street, string? building)
    {
        if (string.IsNullOrWhiteSpace(city))
            return false;
        if (string.IsNullOrWhiteSpace(street))
            return false;
        if (string.IsNullOrWhiteSpace(building))
            return false;

        if (!Regex.IsMatch(city, @"^[a-zA-Z]+$"))
            return false;
        return Regex.IsMatch(street, @"^[a-zA-Z0-9]+$") && Regex.IsMatch(building, @"^[0-9/k]+$");
    }

    public static bool DateTimeCheck(DateTime dateTime)
    {
        return dateTime.CompareTo(CustomDateTime.Now) > 0;
    }

    public static bool BankNameCheck(string name)
    {
        return CentralBank.GetInstance().Banks.All(bank => !bank.Name.Equals(name));
    }
}