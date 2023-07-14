using DataAccess.Interfaces;
using DataAccess.Models.Sources;

namespace DataAccess.Models.Messages;

public class Message
{
    public Message(Guid id, Source senderSource, Source recipientSource, DateTime sendingTime, string content)
    {
        Id = id;
        SenderSource = senderSource;
        RecipientSource = recipientSource;
        SendingTime = sendingTime;
        Status = MessageStatus.Sent;
        Content = content;
    }
    protected Message () { }
    public Guid Id { get; set; }
    public virtual Source? SenderSource { get; set; }
    public virtual Source? RecipientSource { get; set; }
    public DateTime SendingTime { get; set; }
    public MessageStatus Status { get; set; }
    public string? Content { get; set; }
}