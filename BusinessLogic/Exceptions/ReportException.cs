namespace BusinessLogic.Exceptions;

public class ReportException : Exception
{
    public ReportException(string message)
        : base(message) { }
}