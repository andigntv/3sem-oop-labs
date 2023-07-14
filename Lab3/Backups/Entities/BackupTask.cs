using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models.Algorithms;

namespace Backups.Entities;

public class BackupTask
{
    private List<BackupObject> _backupDirectories;
    private List<BackupObject> _backupFiles;

    public BackupTask(IAlgorithm algorithm, IRepository repository, IDirectory directory, IBackup backup)
    {
        if (directory is null)
            throw new ArgumentException("Directory cannot be null");
        if (algorithm is null)
            throw new ArgumentException("Algorithm cannot be null");
        if (repository is null)
            throw new ArgumentException("Repository cannot be null");
        if (backup is null)
            throw new ArgumentException("Backup cannot be null");
        Algorithm = algorithm;
        Repository = repository;
        Directory = directory;
        Backup = backup;
        _backupDirectories = new List<BackupObject>();
        _backupFiles = new List<BackupObject>();
    }

    public IAlgorithm Algorithm { get; }
    public IRepository Repository { get; }
    public IDirectory Directory { get; }
    public IBackup Backup { get; }

    public void AddBackupDirectory(IDirectory directory)
    {
        if (directory is null)
            throw new ArgumentException("Backup object cannot be null");
        string? name = Path.GetDirectoryName(directory.Path);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of backup object cannot be null");
        if (FindBackupObjWithSameName(name) is not null)
            throw new BackupException("Backup object with this name already exists");

        var obj = new BackupObject(name, directory);
        _backupDirectories.Add(obj);
    }

    public void AddBackupFile(IFile file)
    {
        if (file is null)
            throw new ArgumentException("Backup object cannot be null");
        string? name = Path.GetFileName(file.Path);
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of backup object cannot be null");
        if (FindBackupObjWithSameName(name) is not null)
            throw new BackupException("Backup object with this name already exists");

        var obj = new BackupObject(name, file);
        _backupFiles.Add(obj);
    }

    public void RemoveBackupObject(string name)
    {
        BackupObject? obj = FindBackupObjWithSameName(name);
        if (obj is null)
            throw new BackupException("Backup object with this name not found");

        if (!_backupFiles.Remove(obj))
            _backupDirectories.Remove(obj);
    }

    public RestorePoint CreateRestorePoint(string name)
    {
        if (Path.GetInvalidFileNameChars().Any(name.Contains))
            throw new FileException("Invalid Filename");

        IDirectory directory = Repository.CreateDirectory(Path.Combine(Directory.Path, name));
        var backupObjects = new List<BackupObject>();
        backupObjects.AddRange(_backupDirectories);
        backupObjects.AddRange(_backupFiles);
        Storage storage = Algorithm.Archive(backupObjects, Repository, directory);
        var restorePoint = new RestorePoint(name, DateTime.Now, backupObjects, storage);
        Backup.AddRestorePoint(restorePoint);
        return restorePoint;
    }

    public BackupObject? FindBackupObjWithSameName(string name)
    {
        foreach (BackupObject file in _backupFiles.Where(file => file.ShortPath.Equals(name)))
        {
            return file;
        }

        return _backupDirectories.FirstOrDefault(directory => directory.ShortPath.Equals(name));
    }

    public void DeleteRestorePoint(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Backup.DeleteRestorePoint(name);
    }
}