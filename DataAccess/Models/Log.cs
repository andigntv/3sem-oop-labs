namespace DataAccess.Models;

public class Log
{
    public Log(Guid clientId, Guid id, DateTime dateTime, bool logged)
    {
        ClientId = clientId;
        Logged = logged;
        Id = id;
        DateTime = dateTime;
    }
    protected Log() {}
    public bool Logged { get; set; }
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateTime DateTime { get; set; }
}