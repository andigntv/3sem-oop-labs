namespace Backups.Exceptions;

public class RestorePointException : Exception
{
    public RestorePointException(string message)
        : base(message) { }
}