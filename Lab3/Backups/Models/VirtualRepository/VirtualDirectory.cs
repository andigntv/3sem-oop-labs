using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models.VirtualRepository;

public class VirtualDirectory : IDirectory
{
    private List<IFile> _files;
    private List<IDirectory> _directories;

    public VirtualDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path to file cannot be null");

        Path = path;
        _files = new List<IFile>();
        _directories = new List<IDirectory>();
    }

    public string Path { get; }

    public IReadOnlyList<IFile> Files => _files;
    public IReadOnlyList<IDirectory> Directories => _directories;

    public void AddFile(IFile file)
    {
        if (file is null)
            throw new ArgumentException("File cannot be null");
        if (!file.Path.StartsWith(Path))
            throw new FileException("File is not in this directory");
        if (_files.Any(x => x.Path.Equals(file.Path)))
            throw new FileException("File already exists");
        _files.Add(file);
    }

    public void AddDirectory(IDirectory directory)
    {
        if (directory is null)
            throw new ArgumentException("Directory cannot be null");
        if (!directory.Path.StartsWith(Path))
            throw new FileException("Directory is not in this directory");
        if (_files.Any(x => x.Path.Equals(directory.Path)))
            throw new FileException("File already exists");
        _directories.Add(directory);
    }

    public VirtualDirectory? FindDirectory(string path)
    {
        return _directories.Find(x => x.Path.Equals(path)) as VirtualDirectory;
    }

    public VirtualFile? FindFile(string path)
    {
        return _files.Find(x => x.Path.Equals(path)) as VirtualFile;
    }

    public void DeleteFile(string path)
    {
        if (_files.RemoveAll(x => x.Path.Equals(path)) != 1)
            throw new FileNotFoundException("File not found");
    }

    public void DeleteDirectory(string path)
    {
        if (_directories.RemoveAll(x => x.Path.Equals(path)) != 1)
            throw new DirectoryNotFoundException("Directory not found");
    }
}