namespace Shops.Exceptions;

public class AvailabilityException : Exception
{
    public AvailabilityException(string message)
        : base(message) { }
}