using Backups.Interfaces;

namespace Backups.Entities;

public class Storage
{
    public Storage(IRepository repository, IDirectory directory)
    {
        if (repository is null)
            throw new ArgumentException("Repository cannot be null");
        if (directory is null)
            throw new ArgumentException("Directory cannot be null");

        Repository = repository;
        Directory = directory;
    }

    public IRepository Repository { get; }
    public IDirectory Directory { get; }
}