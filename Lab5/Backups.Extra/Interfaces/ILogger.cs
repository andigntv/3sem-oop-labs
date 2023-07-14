using Backups.Entities;

namespace Backups.Extra.Interfaces;

public interface ILogger
{
    void Log(RestorePoint restorePoint);
}