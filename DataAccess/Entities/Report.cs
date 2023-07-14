using DataAccess.Exceptions;

namespace DataAccess.Entities;

public class Report
{
    public const int MinNonNegativeNumber = 0;
    public Report(Guid id, DateTime start, DateTime end, string info)
    {
        if (start.CompareTo(end) >= 0)
            throw new TimeException("Invalid date");
        Id = id;
        Start = start;
        End = end;
        Info = info;
    }
    protected Report() { }
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Info { get; set; }
}