using System.IO.Compression;
using Backups.Entities;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models.Algorithms;

public class SplitStorageAlgorithm : IAlgorithm
{
    public Storage Archive(List<BackupObject> backupObjects, IRepository repository, IDirectory directory)
    {
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

            IFile file = repository.CreateFile(Path.Combine(directory.Path, obj.ShortPath));
            file.Write(stream);
        }

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