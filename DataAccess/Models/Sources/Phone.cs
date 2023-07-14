using System.Text.RegularExpressions;
using DataAccess.Entities;
using DataAccess.Exceptions;
using DataAccess.Interfaces;

namespace DataAccess.Models.Sources;
/**
public partial class Phone : ISource
{
    public Phone(Guid id, Employee owner, string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentNullException(number);
        if (!MyRegex().IsMatch(number))
            throw new PhoneException("Phone number must contain only numbers or +");
        ArgumentNullException.ThrowIfNull(owner);

        Id = id;
        Owner = owner;
        Number = number;
    }

    public Guid Id { get; set; }
    protected Phone() { }
    private string Number { get; set; }
    public virtual Employee Owner { get; set; }
    public string Info => $"Phone: {Number}";

    [GeneratedRegex("^[0-9+]+$")]
    private static partial Regex MyRegex();
}**/