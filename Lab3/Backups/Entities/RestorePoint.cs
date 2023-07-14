using Backups.Exceptions;

namespace Backups.Entities;

public class RestorePoint
{
    public RestorePoint(string name, DateTime date, IReadOnlyList<BackupObject> backupObjects, Storage storage)
    {
        if (Path.GetInvalidFileNameChars().Any(name.Contains))
            throw new FileException("Invalid restore point name");
        if (backupObjects is null)
            throw new ArgumentException("List of BackupObjects cannot be null");
        if (storage is null)
            throw new ArgumentException("Storage cannot be null");
        Name = name;
        Date = date;
        BackupObjects = backupObjects;
        Storage = storage;
    }

    public string Name { get; }
    public DateTime Date { get; private set; }
    public IReadOnlyList<BackupObject> BackupObjects { get; }
    public Storage Storage { get; }

    public void ChangeDate(DateTime dateTime)
    {
        Date = dateTime;
    }

    public string Info()
    {
        string temp = BackupObjects.Aggregate(string.Empty, (current, backupObject) => current + $"${backupObject.ShortPath}\n");

        return $"name: {Name}\n" +
               $"date and time {Date}\n" +
               $"objects:\n" +
               $"{temp}";
    }
}