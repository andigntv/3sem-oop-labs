using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Extra.Entities;

public class BackupExtra : IBackup
{
    private Backup _backup;

    public BackupExtra(Backup backup)
    {
        ArgumentNullException.ThrowIfNull(backup);
        _backup = backup;
    }

    public IReadOnlyList<RestorePoint> RestorePoints => _backup.RestorePoints;
    public void AddRestorePoint(RestorePoint restorePoint) => _backup.AddRestorePoint(restorePoint);
    public RestorePoint? FindRestorePoint(string name) => _backup.FindRestorePoint(name);
    public void DeleteRestorePoint(string name) => _backup.DeleteRestorePoint(name);
    public void DeleteFirst() => _backup.DeleteFirst();
}