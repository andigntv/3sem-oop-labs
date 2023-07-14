using BusinessLogic.Dto;
using DataAccess.Models.Sources;

namespace BusinessLogic.Mapping;

public static class SourceMapping
{
    public static SourceDto AsDto(this Source source)
        => new SourceDto(source.Id, source.Owner!.Id, source.Type!, source.Info!);
}