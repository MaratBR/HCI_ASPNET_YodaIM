using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    public static class Dto
    {
        public static ChatMessageDto CreateMessage(Message message, List<Guid> attachments) => new ChatMessageDto
        {
            Text = message.Text,
            SenderId = message.SenderId,
            Id = message.Id,
            Attachments = attachments,
            RoomId = message.RoomId
        };

        public static MessageAckDto CreateMessageAck(Message message, Guid stamp) => new MessageAckDto
        {
            Id = message.Id,
            Stamp = stamp
        };

        public static UserActionDto CreateUserAction(Guid userId, Guid roomId, UserActionType type) => new UserActionDto
        {
            ActionType = type,
            UserId = userId,
            RoomId = roomId
        };
    }
}
