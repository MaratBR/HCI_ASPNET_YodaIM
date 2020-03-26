using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services.Database
{
    public class MessageService : DatabaseServiceBase, IMessageService
    {
        private readonly IRoomService _roomService;

        public MessageService(Context context, IRoomService roomService) : base(context)
        {
            _roomService = roomService;
        }

        public async Task<Result<Message>> CreateMessage(User sender, Guid roomId, string text, IEnumerable<FileModel> attachments = null)
        {
            if (!await _roomService.Exists(roomId))
                return Results.Fail<Message>($"Room with ID = {roomId} not found");

            var message = new Message
            {
                SenderId = sender.Id,
                RoomId = roomId,
                Text = text
            };
            message.MessageAttachments = attachments.Select(
                fm => new MessageAttachment
                {
                    Message = message,
                    FileModelId = fm.Id
                }
                ).ToList();
            context.Messages.Add(message);
            try
            {
                await context.SaveChangesAsync();
            }
            catch(DbUpdateException exc)
            {
                return Results.Fail<Message>("Failed to save the message: " + exc.Message);
            }

            return Results.Ok<Message>(message);
        }
    }
}
