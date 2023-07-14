namespace Presentation.Models;

public record MessageModel(Guid SenderSourceId, Guid RecipientSourceId, string Content);