using Backups.Interfaces;

namespace Backups.Models.RealRepository;

public class RealFile : IFile
{
    public RealFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path to file cannot be null");

        Path = path;
    }

    public string Path { get; }

    public void Read(Stream ostream)
    {
        if (ostream is null)
            throw new ArgumentException("Stream cannot be null");

        FileStream stream = File.OpenRead(Path);
        stream.CopyTo(ostream);
        stream.Close();
    }

    public void Write(Stream istream)
    {
        if (istream is null)
            throw new ArgumentException("Stream cannot be null");

        FileStream stream = File.Create(Path);
        istream.Seek(0, SeekOrigin.Begin);
        istream.CopyTo(stream);
        stream.Close();
    }
}