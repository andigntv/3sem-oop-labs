namespace Presentation.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message)
        : base(message) { }
}