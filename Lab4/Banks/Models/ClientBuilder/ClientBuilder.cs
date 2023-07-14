using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Models.ClientBuilder;

public class ClientBuilder
{
    private const int MinNonNegativeNumber = 0;
    private string _firstName = "default";
    private string _secondName = "default";
    private int _id;
    private Address? _address = null;
    private Passport? _passport = null;
    private Client _client = new ("Default", "Default", int.MaxValue); // Cause Rider swears that field is uninitialized

    public void BuildNameAndId(string firstName, string secondName, int id)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentNullException();
        if (string.IsNullOrWhiteSpace(secondName))
            throw new ArgumentNullException();
        if (id <= MinNonNegativeNumber)
            throw new ClientException("Id must be positive");

        _firstName = firstName;
        _secondName = secondName;
        _id = id;
    }

    public void BuildPassport(Passport passport)
    {
        ArgumentNullException.ThrowIfNull(passport);
        _passport = passport;
    }

    public void BuildAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        _address = address;
    }

    public Client GetClient()
    {
        _client = new (_firstName, _secondName, _id, _passport, _address);
        return _client;
    }
}