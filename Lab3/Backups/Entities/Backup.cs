using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class Backup : IBackup
{
    public const int IndexOfFirstElement = 0;
    private List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyList<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (FindRestorePoint(restorePoint.Name) is not null)
            throw new RestorePointException("Restore point with this name already exists");
        _restorePoints.Add(restorePoint);
    }

    public RestorePoint? FindRestorePoint(string name)
    {
        return _restorePoints.FirstOrDefault(point => point.Name.Equals(name));
    }

    public void DeleteRestorePoint(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);

        RestorePoint? restorePoint = FindRestorePoint(name);
        if (restorePoint is null)
            throw new RestorePointException("Cannot find restore point");

        restorePoint.Storage.Repository.DeleteDirectory(restorePoint.Storage.Directory.Path);
        if (!_restorePoints.Remove(restorePoint))
            throw new RestorePointException("Cannot find restore point");
    }

    public void DeleteFirst()
    {
        _restorePoints.Remove(_restorePoints[IndexOfFirstElement]);
    }
}