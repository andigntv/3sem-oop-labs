namespace Shops.Exceptions;

public class ExistenceException : Exception
{
    public ExistenceException(string message)
        : base(message) { }
}