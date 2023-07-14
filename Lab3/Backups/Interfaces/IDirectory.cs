namespace Backups.Interfaces;

public interface IDirectory : IRepoObj
{
    public IReadOnlyList<IFile> Files { get; }
    public IReadOnlyList<IDirectory> Directories { get; }
}