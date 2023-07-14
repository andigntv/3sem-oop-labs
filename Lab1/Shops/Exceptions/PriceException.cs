namespace Shops.Exceptions;

public class PriceException : Exception
{
    public PriceException(string message)
        : base(message) { }
}