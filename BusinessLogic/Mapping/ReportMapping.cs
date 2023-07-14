using BusinessLogic.Dto;
using DataAccess.Entities;

namespace BusinessLogic.Mapping;

public static class ReportMapping
{
    public static ReportDto AsDto(this Report report)
        => new ReportDto(report.Id, report.Start, report.End, report.Info);
}