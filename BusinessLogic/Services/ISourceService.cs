using BusinessLogic.Dto;

namespace BusinessLogic.Services;

public interface ISourceService
{
    Task<SourceDto> CreateSourceAsync(Guid ownerId, string type, string info, CancellationToken cancellationToken);
}