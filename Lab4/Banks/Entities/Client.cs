using Banks.Entities.Accounts;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Entities;

public class Client
{
    private const double MinNonNegativeNumber = 0;
    private List<IAccount> _accounts;
    private List<string> _messages;
    public Client(string firstName, string secondName, int id, Passport? passport = null, Address? address = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(secondName))
            throw new ArgumentException("Second name cannot be empty");
        if (id <= MinNonNegativeNumber)
            throw new ClientException("Id cannot be negative");

        FirstName = firstName;
        SecondName = secondName;
        Id = id;
        _accounts = new List<IAccount>();
        _messages = new List<string>();
        if (passport is not null)
            Passport = passport;
        if (address is not null)
            Address = address;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string SecondName { get; }
    public Passport? Passport { get; private set; }
    public Address? Address { get; private set; }
    public IReadOnlyList<IAccount> Accounts => _accounts;
    public bool Subscribed { get; private set; }

    public void AddAccount(IAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);
        if (!account.Owner.Equals(this))
            throw new AccountException("Account doesn't belong to this client");
        _accounts.Add(account);
    }

    public void DeleteAccount(int id)
    {
        IAccount? account = FindAccount(id);
        if (account is null)
            throw new ClientException("No such account");
        _accounts.Remove(account);
    }

    public IAccount? FindAccount(int id)
    {
        return _accounts.Find(x => x.Id == id);
    }

    public void Notify(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException();
        if (!Subscribed)
            throw new ClientException("Client is unsubscribed");
        _messages.Add(message);
    }

    public void SetPassport(Passport passport)
    {
        ArgumentNullException.ThrowIfNull(passport);
        Passport = passport;
    }

    public void SetAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        Address = address;
    }
}