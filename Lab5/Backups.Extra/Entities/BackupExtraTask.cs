using System.Text.Json;
using Backups.Entities;
using Backups.Extra.Exceptions;
using Backups.Extra.Interfaces;
using Backups.Extra.Models;
using Backups.Extra.Models.Serializers;
using Backups.Interfaces;
using Backups.Models.Algorithms;

namespace Backups.Extra.Entities;

public class BackupExtraTask
{
    public const int MinNonNegativeNumber = 0;
    public const int IndexOfFirstElement = 0;
    public const int IndexOfSecondElement = 1;
    public const int IndexOfLastElementWithoutSpecialSymbol = 1;
    public const int MinLimit = 2;
    public const int DefaultLimitOfRestorePoints = 10;
    public const int DefaultRestorePointLifetime = 365;
    private BackupTask _backupTask;

    public BackupExtraTask(
        IAlgorithm algorithm,
        IRepository repository,
        IDirectory directory,
        IBackup backup,
        ILogger logger,
        OverLimitState overLimitState,
        OverLimitAction overLimitAction,
        int maxRestorePoints = DefaultLimitOfRestorePoints,
        int days = DefaultRestorePointLifetime)
    {
        ArgumentNullException.ThrowIfNull(logger);
        if (maxRestorePoints < MinLimit)
            throw new BackupExtraException("Must be more than two");
        if (days < MinNonNegativeNumber)
            throw new BackupExtraException("Must be positive");
        OverLimitState = overLimitState;
        OverLimitAction = overLimitAction;
        _backupTask = new BackupTask(algorithm, repository, directory, backup);
        Logger = logger;
        MaxRestorePoints = maxRestorePoints;
        RestorePointLifeTime = days;
    }

    public IAlgorithm Algorithm => _backupTask.Algorithm;
    public IRepository Repository => _backupTask.Repository;
    public IDirectory Directory => _backupTask.Directory;
    public IBackup Backup => _backupTask.Backup;
    public OverLimitState OverLimitState { get; set; }
    public OverLimitAction OverLimitAction { get; set; }
    public ILogger Logger { get; private set; }
    public int MaxRestorePoints { get; private set; }
    public int RestorePointLifeTime { get; private set; }

    public void AddBackupDirectory(IDirectory directory) => _backupTask.AddBackupDirectory(directory);
    public void AddBackupFile(IFile file) => _backupTask.AddBackupFile(file);
    public void RemoveBackupObject(string name) => _backupTask.RemoveBackupObject(name);
    public BackupObject? FindBackupObjWithSameName(string name) => _backupTask.FindBackupObjWithSameName(name);
    public void DeleteRestorePoint(string name) => _backupTask.DeleteRestorePoint(name);
    public RestorePoint CreateRestorePoint(string name)
    {
        RestorePoint result = _backupTask.CreateRestorePoint(name);
        Update();
        Logger.Log(result);
        return result;
    }

    public void Restore(RestorePoint restorePoint)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        var files = restorePoint.Storage.Directory.Files as List<IFile>;
        ArgumentNullException.ThrowIfNull(files);
        foreach (BackupObject backupObject in restorePoint.BackupObjects)
        {
            Repository.DeleteFile(backupObject.RepoObj.Path);
            IFile file = Repository.CreateFile(backupObject.RepoObj.Path);
            string path = Path.Combine(restorePoint.Storage.Directory.Path, backupObject.ShortPath);
            IFile? source = files.Find(x => x.Path.Equals(path));
            ArgumentNullException.ThrowIfNull(source);

            var temp = new MemoryStream();
            source.Read(temp);
            file.Write(temp);
        }
    }

    public void Restore(RestorePoint restorePoint, IDirectory destinationDirectory)
    {
        ArgumentNullException.ThrowIfNull(restorePoint);

        var files = restorePoint.Storage.Directory.Files as List<IFile>;
        ArgumentNullException.ThrowIfNull(files);
        foreach (BackupObject backupObject in restorePoint.BackupObjects)
        {
            string path = Path.Combine(destinationDirectory.Path, backupObject.ShortPath);
            Repository.DeleteFile(path);
            IFile file = Repository.CreateFile(path);
            string restorePath = Path.Combine(restorePoint.Storage.Directory.Path, backupObject.ShortPath);
            IFile? source = files.Find(x => x.Path.Equals(path));
            ArgumentNullException.ThrowIfNull(source);

            var temp = new MemoryStream();
            source.Read(temp);
            file.Write(temp);
        }
    }

    public void Merge(RestorePoint firstRestorePoint)
    {
        ArgumentNullException.ThrowIfNull(firstRestorePoint);
        RestorePoint lastRestorePoint = _backupTask.Backup.RestorePoints[^IndexOfLastElementWithoutSpecialSymbol];

        var difference = firstRestorePoint.BackupObjects.Where(backupObject => !lastRestorePoint.BackupObjects.Contains(backupObject)).ToList();

        string name = $"{lastRestorePoint.Name}_{firstRestorePoint.Name}";
        IDirectory directory = Repository.CreateDirectory(Path.Combine(Directory.Path, name));

        var backupObjects = new List<BackupObject>();
        backupObjects.AddRange(lastRestorePoint.BackupObjects);
        backupObjects.AddRange(difference);

        Storage storage = Algorithm.Archive(backupObjects, Repository, directory);
        var restorePoint = new RestorePoint(name, DateTime.Now, backupObjects, storage);

        Backup.AddRestorePoint(restorePoint);
        _backupTask.DeleteRestorePoint(firstRestorePoint.Name);
    }

    public void MergeFirst()
    {
        RestorePoint firstRestorePoint = _backupTask.Backup.RestorePoints[IndexOfFirstElement];
        RestorePoint lastRestorePoint = _backupTask.Backup.RestorePoints[^IndexOfLastElementWithoutSpecialSymbol];

        var difference = firstRestorePoint.BackupObjects.Where(backupObject => !lastRestorePoint.BackupObjects.Contains(backupObject)).ToList();

        string name = $"{lastRestorePoint.Name}_{firstRestorePoint.Name}";
        IDirectory directory = Repository.CreateDirectory(Path.Combine(Directory.Path, name));

        var backupObjects = new List<BackupObject>();
        backupObjects.AddRange(lastRestorePoint.BackupObjects);
        backupObjects.AddRange(difference);

        Storage storage = Algorithm.Archive(backupObjects, Repository, directory);
        var restorePoint = new RestorePoint(name, DateTime.Now, backupObjects, storage);

        Backup.AddRestorePoint(restorePoint);
        _backupTask.DeleteRestorePoint(firstRestorePoint.Name);
    }

    public void MergeFirstTwo()
    {
        RestorePoint firstRestorePoint = _backupTask.Backup.RestorePoints[IndexOfFirstElement];
        RestorePoint secondRestorePoint = _backupTask.Backup.RestorePoints[IndexOfSecondElement];
        RestorePoint lastRestorePoint = _backupTask.Backup.RestorePoints[^IndexOfLastElementWithoutSpecialSymbol];

        var difference = firstRestorePoint.BackupObjects.Where(backupObject => !lastRestorePoint.BackupObjects.Contains(backupObject)).ToList();

        var backupObjects = new List<BackupObject>();
        backupObjects.AddRange(lastRestorePoint.BackupObjects);
        backupObjects.AddRange(difference);

        difference = secondRestorePoint.BackupObjects.Where(backupObject => !backupObjects.Contains(backupObject)).ToList();
        backupObjects.AddRange(difference);
        string name = $"{lastRestorePoint.Name}_{firstRestorePoint.Name}_{secondRestorePoint.Name}";
        IDirectory directory = Repository.CreateDirectory(Path.Combine(Directory.Path, name));

        Storage storage = Algorithm.Archive(backupObjects, Repository, directory);
        var restorePoint = new RestorePoint(name, DateTime.Now, backupObjects, storage);

        Backup.AddRestorePoint(restorePoint);
        _backupTask.DeleteRestorePoint(firstRestorePoint.Name);
        _backupTask.DeleteRestorePoint(secondRestorePoint.Name);
    }

    public void SetMaxRestorePoints(int maxRestorePoints)
    {
        if (maxRestorePoints < MinNonNegativeNumber)
            throw new BackupExtraException("Must be positive");
        MaxRestorePoints = maxRestorePoints;
        Update();
    }

    public void SetRestorePointLifeTime(int days)
    {
        if (days < MinNonNegativeNumber)
            throw new BackupExtraException("Must be positive");
        RestorePointLifeTime = days;
        Update();
    }

    public void ChangeLogger(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
    }

    public bool OverLifetime(RestorePoint restorePoint)
    {
        TimeSpan difference = DateTime.Now.Subtract(restorePoint.Date);
        return difference.Days > RestorePointLifeTime;
    }

    public void Update()
    {
        switch (OverLimitState)
        {
            case OverLimitState.OnlyAmount:
                if (OverLimitAction is OverLimitAction.Delete)
                {
                    while (Backup.RestorePoints.Count > MaxRestorePoints)
                        Backup.DeleteFirst();
                }
                else
                {
                    while (Backup.RestorePoints.Count > MaxRestorePoints)
                        MergeFirstTwo();
                }

                break;
            case OverLimitState.OnlyLifetime:
                if (OverLimitAction is OverLimitAction.Delete)
                {
                    while (OverLifetime(Backup.RestorePoints[IndexOfFirstElement]))
                        Backup.DeleteFirst();
                }
                else
                {
                    while (OverLifetime(Backup.RestorePoints[IndexOfFirstElement]))
                        MergeFirst();
                }

                break;
            case OverLimitState.Any:
                if (OverLimitAction is OverLimitAction.Delete)
                {
                    while (Backup.RestorePoints.Count > MaxRestorePoints)
                        Backup.DeleteFirst();

                    while (OverLifetime(Backup.RestorePoints[IndexOfFirstElement]))
                        Backup.DeleteFirst();
                }
                else
                {
                    while (Backup.RestorePoints.Count > MaxRestorePoints)
                        MergeFirstTwo();

                    while (OverLifetime(Backup.RestorePoints[IndexOfFirstElement]))
                        MergeFirst();
                }

                break;
            case OverLimitState.Together:
                if (OverLimitAction is OverLimitAction.Delete)
                {
                    while (Backup.RestorePoints.Count > MaxRestorePoints && OverLifetime(Backup.RestorePoints[IndexOfFirstElement]))
                        Backup.DeleteFirst();
                }
                else
                {
                    while (Backup.RestorePoints.Count > MaxRestorePoints && OverLifetime(Backup.RestorePoints[IndexOfFirstElement]))
                        MergeFirstTwo();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string Serialize()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new AlgoSerializer());
        options.Converters.Add(new RepoSerializer());
        options.Converters.Add(new FileSerializer());
        options.Converters.Add(new LoggerSerializer());
        return JsonSerializer.Serialize(this, options);
    }
}