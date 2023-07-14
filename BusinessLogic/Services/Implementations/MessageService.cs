using BusinessLogic.Dto;
using BusinessLogic.Exceptions;
using BusinessLogic.Extensions;
using BusinessLogic.Mapping;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models;
using DataAccess.Models.Messages;
using DataAccess.Models.Sources;

namespace BusinessLogic.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly DatabaseContext _context;
    public MessageService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<MessageDto> CreateMessageAsync(
        Guid senderId, 
        Guid receiverId, 
        string content, 
        CancellationToken cancellationToken)
    {
        Source sender = await _context.Sources.GetEntityAsync(senderId, cancellationToken);
        Source receiver = await _context.Sources.GetEntityAsync(receiverId, cancellationToken);
        
        if (!sender.Type!.Equals(receiver.Type))
            throw new ReportException("Source types mismatch");
        
        var message = new Message(Guid.NewGuid(), sender, receiver, DateTime.Now, content);
        
        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);
        
        return message.AsDto();
    }

    public async Task<MessageDto> ReadMessageAsync(Guid id, CancellationToken cancellationToken)
    {
        Message message = await _context.Messages.GetEntityAsync(id, cancellationToken);
        message.Status = MessageStatus.Delivered;
        await _context.SaveChangesAsync(cancellationToken);
        return message.AsDto();
    }
    public async Task<MessageDto> ReplyToMessageAsync(Guid id, string content,
        CancellationToken cancellationToken)
    {
        Message message = await _context.Messages.GetEntityAsync(id, cancellationToken);
        message.Status = MessageStatus.Processed;
        await _context.SaveChangesAsync(cancellationToken);
        return await CreateMessageAsync(message.RecipientSource!.Id, message.SenderSource!.Id, content, cancellationToken);
    }
    
    public async Task<List<MessageDto>> UnreadMessagesAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.GetEntityAsync(employeeId, cancellationToken);
        var messages = _context.Messages.Where(x =>
            x.Status == MessageStatus.Sent && x.RecipientSource!.Owner!.Id.Equals(employeeId)).ToList();
        
        return messages.Select(x => x.AsDto()).ToList();
    }

    public async Task<List<MessageDto>> UnprocessedMessagesAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.GetEntityAsync(employeeId, cancellationToken);
        var messages = _context.Messages.Where(x =>
            (x.Status == MessageStatus.Sent || x.Status == MessageStatus.Delivered) && x.RecipientSource!.Owner!.Id.Equals(employeeId)).ToList();
        
        return messages.Select(x => x.AsDto()).ToList();
    }

    public async Task<List<MessageDto>> AllMessagesAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees.GetEntityAsync(employeeId, cancellationToken);
        var messages = _context.Messages.Where(x => x.RecipientSource!.Owner!.Id.Equals(employeeId)).ToList();
        
        return messages.Select(x => x.AsDto()).ToList();
    }
    public async Task<ReportDto> CreateReportAsync(DateTime start, CancellationToken cancellationToken)
    {
        var messages = _context.Messages.Where(x => x.SendingTime.CompareTo(start) > 0).ToList();
        var sourceTypes = new List<string>();
        
        foreach (Message message in messages.Where(message => !sourceTypes.Contains(message.RecipientSource!.Type!)))
            sourceTypes.Add(message.RecipientSource!.Type!);

        string result = string.Empty;
        foreach (string source in sourceTypes)
        {
            var tempMessages = messages.Where(x => x.RecipientSource!.Type!.Equals(source)).ToList();
            int processed = tempMessages.Count(x => x.Status == MessageStatus.Processed);
            int delivered = processed + tempMessages.Count(x => x.Status == MessageStatus.Delivered);
            int sent = delivered + tempMessages.Count(x => x.Status == MessageStatus.Sent);
            result += $"Source Type : {source}\n" +
                      $"Messages Processed {processed} Delivered {delivered} Sent {sent}\n" +
                      $"-------------------------------------------------\n";
        }
        
        var report = new Report(Guid.NewGuid(), start, DateTime.Now, result);

        _context.Reports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);

        return report.AsDto();
    }
}