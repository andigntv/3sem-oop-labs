using DataAccess.Models;

namespace BusinessLogic.Dto;

public record MessageDto(Guid Id, string Content, MessageStatus Status, SourceDto SenderSource, SourceDto RecipientSource);