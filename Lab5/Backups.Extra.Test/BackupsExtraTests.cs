using System.IO.Compression;
using Backups.Entities;
using Backups.Extra.Entities;
using Backups.Extra.Models;
using Backups.Interfaces;
using Backups.Models.Algorithms;
using Backups.Models.RealRepository;
using Backups.Models.VirtualRepository;
using Xunit;
using Xunit.Abstractions;

namespace Backups.Extra.Test;

public class BackupsExtraTaskTest
{
    [Fact]
    public void MergeTest()
    {
        var repository = new VirtualRepository();
        IDirectory backupDirectory = repository.CreateDirectory(Path.Combine("root", "backups"));
        var algorithm = new SplitStorageAlgorithm();
        var backup = new Backup();
        var backupTask = new BackupExtraTask(algorithm, repository, backupDirectory, backup, new ConsoleLogger(), OverLimitState.Together, OverLimitAction.Delete);

        IDirectory sourceDirectory = repository.CreateDirectory(Path.Combine("root", "source"));
        IFile fileA = repository.CreateFile(Path.Combine("root", "source", "A.txt"));
        IFile fileB = repository.CreateFile(Path.Combine("root", "source", "B.txt"));
        IFile fileC = repository.CreateFile(Path.Combine("root", "source", "C.txt"));

        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        streamWriter.Write("Some information lalalalalalalala");
        streamWriter.Flush();
        fileA.Write(stream);
        backupTask.AddBackupFile(fileA);
        streamWriter.Write("Some information lblblblblblblblb");
        streamWriter.Flush();
        fileB.Write(stream);
        backupTask.AddBackupFile(fileB);
        streamWriter.Write("Some information lclclclclclclclc");
        streamWriter.Flush();
        fileC.Write(stream);

        RestorePoint firstRestorePoint = backupTask.CreateRestorePoint("First");
        IDirectory firstRestorePointDirectory = repository.GetDirectory(Path.Combine("root", "backups", "First")); // first restore point: fileA and fileB

        Assert.Equal(2, firstRestorePointDirectory.Files.Count);

        backupTask.AddBackupFile(fileC);
        backupTask.RemoveBackupObject("B.txt");

        backupTask.CreateRestorePoint("Second");
        IDirectory secondRestorePointDirectory = repository.GetDirectory(Path.Combine("root", "backups", "Second")); // second restore point: fileA and fileC

        Assert.Equal(2, secondRestorePointDirectory.Files.Count);

        backupTask.Merge(firstRestorePoint);

        IDirectory mergedRestorePointDirectory = repository.GetDirectory(Path.Combine("root", "backups", "Second_First"));

        Assert.Equal(3, mergedRestorePointDirectory.Files.Count); // as you see first point was successfully merged in second
    }
}