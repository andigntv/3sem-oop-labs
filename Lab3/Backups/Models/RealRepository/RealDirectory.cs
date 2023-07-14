using Backups.Interfaces;

namespace Backups.Models.RealRepository;

public class RealDirectory : IDirectory
{
    public RealDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path to file cannot be null");

        Path = path;
    }

    public string Path { get; }

    public IReadOnlyList<IFile> Files
    {
        get
        {
            string[] allFiles = Directory.GetFiles(Path);
            IReadOnlyCollection<IFile> result = allFiles.Select(x => new RealFile(x)).ToList();
            return result.ToList();
        }
    }

    public IReadOnlyList<IDirectory> Directories
    {
        get
        {
            string[] allDirectories = Directory.GetDirectories(Path);
            IReadOnlyCollection<IDirectory> result = allDirectories.Select(x => new RealDirectory(x)).ToList();
            return result.ToList();
        }
    }
}