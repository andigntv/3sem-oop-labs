using Backups.Entities;

namespace Backups.Interfaces;

public interface IBackup
{
    IReadOnlyList<RestorePoint> RestorePoints { get; }
    void AddRestorePoint(RestorePoint restorePoint);
    RestorePoint? FindRestorePoint(string name);
    void DeleteRestorePoint(string name);
    void DeleteFirst();
}