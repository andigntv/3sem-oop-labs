using System.IO.Compression;
using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models.Algorithms;

public class SingleStorageAlgorithm : IAlgorithm
{
    public Storage Archive(List<BackupObject> backupObjects, IRepository repository, IDirectory directory)
    {
        IDirectory temp = repository.CreateDirectory(Path.Combine(directory.Path, "temp"));
        foreach (BackupObject obj in backupObjects)
        {
            string? directoryName = Path.GetDirectoryName(obj.RepoObj.Path);
            if (string.IsNullOrWhiteSpace(directoryName))
                throw new FileException("Directory name cannot be null");

            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                switch (obj.RepoObj)
                {
                    case IFile repoObj:
                        ArchiveFile(repoObj, archive, directoryName.Length + 1);
                        break;
                    case IDirectory objRepoObj when objRepoObj.Path.Last().Equals(Path.DirectorySeparatorChar):
                        ArchiveDirectory(objRepoObj, archive, directoryName.Length + 1);
                        break;
                    case IDirectory objRepoObj:
                        ArchiveDirectory(objRepoObj, archive, directoryName.Length);
                        break;
                }
            }

            IFile file = repository.CreateFile(Path.Combine(temp.Path, obj.ShortPath));
            file.Write(stream);
        }

        IFile zipFile = repository.CreateFile(Path.Combine(directory.Path, "archive.zip"));
        var zipStream = new MemoryStream();

        string? name = Path.GetDirectoryName(temp.Path);
        if (string.IsNullOrWhiteSpace(name))
            throw new FileException("Directory name cannot be null");
        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Update, true))
        {
            ArchiveDirectory(temp, zipArchive, name.Length);
        }

        // zipStream.Seek(0, SeekOrigin.Begin);
        zipFile.Write(zipStream);
        repository.DeleteDirectory(temp.Path);
        return new Storage(repository, directory);
    }

    private void ArchiveDirectory(IDirectory directory, ZipArchive archive, int localPathStart)
    {
        foreach (IFile file in directory.Files)
        {
            ArchiveFile(file, archive, localPathStart);
        }

        foreach (IDirectory dir in directory.Directories)
        {
            ArchiveDirectory(dir, archive, localPathStart);
        }
    }

    private void ArchiveFile(IFile file, ZipArchive archive, int localPathStart)
    {
        ZipArchiveEntry entry = archive.CreateEntry(file.Path.Substring(localPathStart));
        Stream entryStream = entry.Open();
        file.Read(entryStream);
    }
}