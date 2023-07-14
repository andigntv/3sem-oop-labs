using BusinessLogic.Dto;
using DataAccess.Models.Messages;

namespace BusinessLogic.Mapping;

public static class MessageMapping
{
    public static MessageDto AsDto(this Message message)
        => new MessageDto(message.Id, message.Content, message.Status, message.SenderSource.AsDto(), message.RecipientSource.AsDto());

}