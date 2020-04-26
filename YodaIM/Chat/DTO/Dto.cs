using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    public static class Dto
    {
        public static ChatMessageDto CreateMessage(Message message, List<FileModel> attachments) => new ChatMessageDto
        {
            Text = message.Text,
            Sender = CreateSenderDto(message.Sender),
            Id = message.Id,
            Attachments = attachments.Select(
                a => new ChatAttachmentDto
                {
                    Id = a.Id,
                    Name = a.FileName,
                    Size = a.Size
                }).ToList(),
            RoomId = message.RoomId,
            PublishedAt = message.PublishedAt
        };

        public static MessageAckDto CreateMessageAck(Message message, Guid stamp) => new MessageAckDto
        {
            Id = message.Id,
            Stamp = stamp
        };

        public static UserActionDto CreateUserAction(int userId, Guid roomId, UserActionType type) => new UserActionDto
        {
            ActionType = type,
            UserId = userId,
            RoomId = roomId
        };

        public static ChatMessageSenderDto CreateSenderDto(User user) => new ChatMessageSenderDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        public static ChatUserDto CreateChatUser(User user) => new ChatUserDto
        {
            Name = user.UserName,
            Id = user.Id,
            Gender = user.Gender
        };

        public static UserStatusChanged CreateUserJoinedDto(User user) => new UserStatusChanged
        {
            UserId = user.Id,
            IsOnline = true
        };

        public static UserStatusChanged CreateUserDepartedDto(User user) => new UserStatusChanged
        {
            UserId = user.Id,
            IsOnline = false
        };
    }
}
