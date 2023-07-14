using Backups.Interfaces;

namespace Backups.Models.RealRepository;

public class RealRepository : IRepository
{
    public IFile CreateFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be empty");

        string? directoryName = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(directoryName))
            throw new ArgumentException("Invalid name of file");

        if (!Directory.Exists(Path.GetDirectoryName(path)))
            new DirectoryInfo(directoryName).Create();
        if (!File.Exists(path))
            new FileInfo(path).Create().Dispose();

        return new RealFile(path);
    }

    public IDirectory CreateDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be empty");

        string? directoryName = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(directoryName))
            throw new ArgumentException("Invalid name of file");

        if (!Directory.Exists(Path.GetDirectoryName(path)))
            new DirectoryInfo(directoryName).Create();

        return new RealDirectory(path);
    }

    public void DeleteFile(string path)
    {
        if (!File.Exists(path))
            return;
        File.Delete(path);
    }

    public void DeleteDirectory(string path)
    {
        if (!Directory.Exists(path))
            return;
        Directory.Delete(path, true);
    }
}