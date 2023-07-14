using Backups.Entities;
using Backups.Extra.Interfaces;

namespace Backups.Extra.Models;

public class ConsoleLogger : ILogger
{
    public void Log(RestorePoint restorePoint)
    {
        Console.WriteLine(restorePoint.Info());
    }
}