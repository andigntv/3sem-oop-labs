namespace Backups.Interfaces;

public interface IFile : IRepoObj
{
    public void Read(Stream ostream);
    public void Write(Stream istream);
}