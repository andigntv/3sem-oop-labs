namespace Shops.Exceptions;

public class AmountException : Exception
{
    public AmountException(string message)
        : base(message) { }
}