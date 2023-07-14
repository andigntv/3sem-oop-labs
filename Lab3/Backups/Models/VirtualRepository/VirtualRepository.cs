using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models.VirtualRepository;

public class VirtualRepository : IRepository
{
    private VirtualDirectory _root;
    public VirtualRepository()
    {
        _root = new VirtualDirectory("root");
    }

    public IFile GetFile(string path)
    {
        IFile? result = FindFile(path);
        if (result is null)
            throw new FileNotFoundException("File not found");
        return result;
    }

    public IDirectory GetDirectory(string path)
    {
        IDirectory? result = FindDirectory(path);
        if (result is null)
            throw new DirectoryNotFoundException("Directory not found");
        return result;
    }

    public VirtualFile? FindFile(string path)
    {
        string[] directories = path.Split(Path.DirectorySeparatorChar);
        if (directories.Length == 0)
            return null;

        VirtualDirectory currentDirectory = _root;
        string currentPath = _root.Path;

        for (int i = 1; i < directories.Length - 1; ++i)
        {
            currentPath = Path.Combine(currentPath, directories[i]);
            VirtualDirectory? temp = currentDirectory.FindDirectory(currentPath);
            if (temp is null)
                return null;
            currentDirectory = temp;
        }

        return currentDirectory.FindFile(path);
    }

    public VirtualDirectory? FindDirectory(string path)
    {
        string[] directories = path.Split(Path.DirectorySeparatorChar);
        if (directories.Length == 0)
            return null;

        VirtualDirectory currentDirectory = _root;
        string currentPath = _root.Path;

        for (int i = 1; i < directories.Length; ++i)
        {
            currentPath = Path.Combine(currentPath, directories[i]);
            VirtualDirectory? temp = currentDirectory.FindDirectory(currentPath);
            if (temp is null)
                return null;
            currentDirectory = temp;
        }

        return currentDirectory;
    }

    public IFile CreateFile(string path)
    {
        VirtualDirectory parent = GetParent(path);
        var file = new VirtualFile(path);
        parent.AddFile(file);
        return file;
    }

    public IDirectory CreateDirectory(string path)
    {
        VirtualDirectory parent = GetParent(path);
        var directory = new VirtualDirectory(path);
        parent.AddDirectory(directory);
        return directory;
    }

    public void DeleteFile(string path)
    {
        VirtualDirectory parent = GetParent(path);
        parent.DeleteFile(path);
    }

    public void DeleteDirectory(string path)
    {
        VirtualDirectory parent = GetParent(path);
        parent.DeleteDirectory(path);
    }

    public VirtualDirectory GetParent(string path)
    {
        VirtualDirectory? result = FindParent(path);
        if (result is null)
            throw new DirectoryNotFoundException("Parent directory not found");
        return result;
    }

    public VirtualDirectory? FindParent(string path)
    {
        string? parentPath = Path.GetDirectoryName(path);
        return string.IsNullOrWhiteSpace(parentPath) ? null : FindDirectory(parentPath);
    }
}