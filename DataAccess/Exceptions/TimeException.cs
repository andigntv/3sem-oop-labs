namespace DataAccess.Exceptions;

public class TimeException: Exception
{
    public TimeException(string message)
        : base(message) { }
}