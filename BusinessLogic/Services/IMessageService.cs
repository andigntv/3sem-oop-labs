using BusinessLogic.Dto;

namespace BusinessLogic.Services;

public interface IMessageService
{

    Task<MessageDto> CreateMessageAsync(Guid senderId, Guid receiverId, string content,
        CancellationToken cancellationToken);

    Task<MessageDto> ReadMessageAsync(Guid id, CancellationToken cancellationToken);

    Task<MessageDto> ReplyToMessageAsync(Guid id, string content,
        CancellationToken cancellationToken);

    Task<List<MessageDto>> UnreadMessagesAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<List<MessageDto>> UnprocessedMessagesAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<List<MessageDto>> AllMessagesAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<ReportDto> CreateReportAsync(DateTime start, CancellationToken cancellationToken);
}