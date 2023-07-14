namespace Backups.Interfaces;

public interface IRepository
{
    IFile CreateFile(string path);
    IDirectory CreateDirectory(string path);
    void DeleteFile(string path);
    void DeleteDirectory(string path);
}