using Backups.Entities;
using Backups.Extra.Interfaces;
using Backups.Interfaces;

namespace Backups.Extra.Models;

public class FileLogger : ILogger, IDisposable
{
    private MemoryStream _stream;
    private StreamWriter _writer;
    public FileLogger(IFile file)
    {
        ArgumentNullException.ThrowIfNull(file);
        _stream = new MemoryStream();
        _writer = new StreamWriter(_stream);
        File = file;
    }

    public IFile File { get; private set; }

    public void SetFile(IFile file)
    {
        ArgumentNullException.ThrowIfNull(file);
        _stream.Position = 0;
        File = file;
    }

    public void Log(RestorePoint restorePoint)
    {
        _writer.Write(restorePoint.Info());
        _writer.Flush();
        File.Write(_stream);
    }

    public void Dispose()
    {
        _stream.Dispose();
        _writer.Dispose();
    }
}