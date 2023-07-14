using Backups.Interfaces;

namespace Backups.Models.VirtualRepository;

public class VirtualFile : IFile, IDisposable
{
    private MemoryStream _stream;

    public VirtualFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path to file cannot be null");

        Path = path;
        _stream = new MemoryStream();
    }

    public string Path { get; }

    public void Read(Stream ostream)
    {
        if (ostream is null)
            throw new ArgumentException("Stream cannot be null");

        _stream.CopyTo(ostream);
    }

    public void Write(Stream istream)
    {
        if (istream is null)
            throw new ArgumentException("Stream cannot be null");

        _stream.SetLength(0);
        istream.CopyTo(_stream);
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}