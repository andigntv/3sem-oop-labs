using Banks.Entities;

namespace Banks.Models.ClientBuilder;

public class ClientDirector
{
    public ClientDirector()
    {
        Builder = new ClientBuilder();
    }

    public ClientBuilder Builder { get; }

    public Client BuildClient_OnlyName(int id, string firstName, string secondName)
    {
        Builder.BuildNameAndId(firstName, secondName, id);
        return Builder.GetClient();
    }

    public Client BuildClient_NameAndPassport(int id, string firstName, string secondName, Passport passport)
    {
        Builder.BuildNameAndId(firstName, secondName, id);
        Builder.BuildPassport(passport);
        return Builder.GetClient();
    }

    public Client BuildClient_NameAndAddress(int id, string firstName, string secondName, Address address)
    {
        Builder.BuildNameAndId(firstName, secondName, id);
        Builder.BuildAddress(address);
        return Builder.GetClient();
    }

    public Client BuildClient_Full(int id, string firstName, string secondName, Passport passport, Address address)
    {
        Builder.BuildNameAndId(firstName, secondName, id);
        Builder.BuildPassport(passport);
        Builder.BuildAddress(address);
        return Builder.GetClient();
    }
}