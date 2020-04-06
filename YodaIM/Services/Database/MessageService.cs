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

        public async Task<Result<Message>> CreateMessage(User sender, Guid roomId, string text, ICollection<FileModel> attachments = null)
        {
            if (!await _roomService.Exists(roomId))
                return Results.Fail<Message>($"Room with ID = {roomId} not found");

            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            if (text == null)
                throw new ArgumentNullException(nameof(text));

            context.Attach(sender);
            var message = new Message
            {
                Sender = sender,
                SenderId = sender.Id,
                RoomId = roomId,
                Text = text
            };
            if (attachments != null)
            {
                message.MessageAttachments = attachments.Select(
                fm => new MessageAttachment
                {
                    Message = message,
                    FileModelId = fm.Id
                }
                ).ToList();
            }

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

        public async Task<List<Message>> GetMessages(Guid roomId, int limit = 50, DateTime? beforeUtc = null)
        {
            beforeUtc = beforeUtc ?? DateTime.UtcNow;
            return await context.Messages
                .Where(m => m.RoomId == roomId && m.PublishedAt < beforeUtc)
                .OrderBy(m => m.PublishedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}
