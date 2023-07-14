using Backups.Entities;
using Backups.Interfaces;
using Backups.Models.Algorithms;
using Backups.Models.RealRepository;
using Backups.Models.VirtualRepository;
using Xunit;

namespace Backups.Test;

public class BackupsTests
{
    [Fact]
    public void BackupTaskOnVirtualRepositoryWithSplitAlgo()
    {
        var repository = new VirtualRepository();
        IDirectory backupDirectory = repository.CreateDirectory(Path.Combine("root", "backups"));
        var algorithm = new SplitStorageAlgorithm();
        var backup = new Backup();
        var backupTask = new BackupTask(algorithm, repository, backupDirectory, backup);

        IDirectory sourceDirectory = repository.CreateDirectory(Path.Combine("root", "source"));
        IFile fileA = repository.CreateFile(Path.Combine("root", "source", "A.txt"));
        IFile fileB = repository.CreateFile(Path.Combine("root", "source", "B.txt"));

        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        streamWriter.Write("Some information lalalalalalalala");
        streamWriter.Flush();
        stream.Position = 0;
        fileA.Write(stream);
        backupTask.AddBackupFile(fileA);
        streamWriter.Write("Some information lblblblblblblblb");
        streamWriter.Flush();
        stream.Position = 0;
        fileB.Write(stream);
        backupTask.AddBackupFile(fileB);

        backupTask.CreateRestorePoint("First");
        IDirectory firstRestorePointDirectory = repository.GetDirectory(Path.Combine("root", "backups", "First"));

        Assert.Equal(2, firstRestorePointDirectory.Files.Count);
        Assert.Equal(0, firstRestorePointDirectory.Directories.Count);
        Assert.Equal(0, backupDirectory.Files.Count);
        Assert.Equal(1, backupDirectory.Directories.Count);
        Assert.Equal(2, sourceDirectory.Files.Count);
        Assert.Equal(0, sourceDirectory.Directories.Count);

        backupTask.RemoveBackupObject("B.txt");

        backupTask.CreateRestorePoint("Second");
        IDirectory secondRestorePointDirectory = repository.GetDirectory(Path.Combine("root", "backups", "Second"));

        Assert.Equal(1, secondRestorePointDirectory.Files.Count);
        Assert.Equal(0, secondRestorePointDirectory.Directories.Count);
        Assert.Equal(0, backupDirectory.Files.Count);
        Assert.Equal(2, backupDirectory.Directories.Count);
        Assert.Equal(2, sourceDirectory.Files.Count);
        Assert.Equal(0, sourceDirectory.Directories.Count);
    }
}