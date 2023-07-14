using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;
public class BackupObject
{
    public BackupObject(string shortPath, IRepoObj repoObj)
    {
        if (Path.GetInvalidFileNameChars().Any(shortPath.Contains))
            throw new FileException("Invalid Filename");
        if (repoObj is null)
            throw new ArgumentException("RepoObj cannot be null");

        ShortPath = shortPath;
        RepoObj = repoObj;
    }

    public string ShortPath { get; }
    public IRepoObj RepoObj { get; }
}