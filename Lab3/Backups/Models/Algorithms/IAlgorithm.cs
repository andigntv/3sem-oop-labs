using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Models.Algorithms;

public interface IAlgorithm
{
    Storage Archive(List<BackupObject> backupObjects, IRepository repository, IDirectory directory);
}