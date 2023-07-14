namespace Backups.Extra.Exceptions;

public class BackupExtraException : Exception
{
    public BackupExtraException(string message)
        : base(message) { }
}