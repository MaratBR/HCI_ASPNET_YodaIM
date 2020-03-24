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

        public async Task<Result<Message>> CreateMessage(User sender, int roomId, string text)
        {
            if (!await _roomService.Exists(roomId))
                return Results.Fail<Message>($"Room with ID = {roomId} not found");

            var message = new Message
            {
                SenderId = sender.Id,
                RoomId = roomId,
                Text = text
            };
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

        public async Task<Result<Message>> CreateMessage(User sender, int roomId, FileModel file)
        {
            if (!await _roomService.Exists(roomId))
                return Results.Fail<Message>($"Room with ID = {roomId} not found");

            var message = new Message
            {
                Sender = sender,
                RoomId = roomId,
                File = file
            };
            context.Messages.Add(message);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                return Results.Fail<Message>("Failed to save the message: " + exc.Message);
            }

            return Results.Ok<Message>(message);
        }
    }
}
