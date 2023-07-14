using BusinessLogic.Dto;
using BusinessLogic.Extensions;
using BusinessLogic.Mapping;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.Sources;

namespace BusinessLogic.Services.Implementations;

public class SourceService : ISourceService
{
    private readonly DatabaseContext _context;

    public SourceService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<SourceDto> CreateSourceAsync(Guid ownerId, string type, string info, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.GetEntityAsync(ownerId, cancellationToken);
        var source = new Source(Guid.NewGuid(), employee, type, info);
        
        _context.Sources.Add(source);

        await _context.SaveChangesAsync(cancellationToken);
        return source.AsDto();
    }
    
}