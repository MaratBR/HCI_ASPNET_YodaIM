using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YodaIM.Chat.DTO;
using YodaIM.Helpers;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Chat
{
    [Authorize]
    public class YODAHub : Hub
    {
        private readonly IRoomService _roomService;
        private readonly IFileService _fileService;
        private readonly IMessageService _messageService;
        private readonly IChatState _chatState;
        private readonly UserManager<User> _userManager;

        public YODAHub(
            IChatState chatState,
            IRoomService roomService,
            IMessageService messageService,
            IFileService fileService,
            UserManager<User> userManager)
        {
            _roomService = roomService;
            _userManager = userManager;
            _messageService = messageService;
            _fileService = fileService;
            _chatState = chatState;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var user = await _userManager.GetUserAsync(Context.User);

            if (user == null)
            {
                Disconnect();
                return;
            }

            _chatState.AddConnection(Context.ConnectionId, user);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"U{user.Id}");

            var rooms = await _roomService.ListRooms(user);
            rooms.ForEach(async r =>
            {
                await AddToRoom(user, r.Id);
            });
        }

        private async Task AddToRoom(User user, Guid roomId)
        {
            _chatState.GetUser(Context.ConnectionId).Rooms.Add(roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"R{roomId}");
            await Clients.Group($"R{roomId}").SendAsync("UserStatus", Dto.CreateUserJoinedDto(user));
        }

        private async Task RemoveFromRoom(User user, Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"R{roomId}");
            await Clients.Group($"R{roomId}").SendAsync("UserStatus", Dto.CreateUserDepartedDto(user));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _userManager.GetUserAsync(Context.User);

            if (user != null)
            {
                var chatUser = _chatState.GetUser(Context.ConnectionId);
                var tasks = chatUser.Rooms.Select(roomId =>
                {
                    return RemoveFromRoom(user, roomId);
                });
                await Task.WhenAll(tasks);

            }

            _chatState.RemoveConnection(Context.ConnectionId);


            await base.OnDisconnectedAsync(exception);
        }

        public async Task Send(ChatMessageRequestDto messageDto)
        {
            var user = await _userManager.GetUserAsync(Context.User);

            if (await _roomService.InRoom(user, messageDto.RoomId))
            {
                List<FileModel> attachments;
                Result<Message> message;
                if (messageDto.Attachments.Count() == 0)
                {
                    message = await _messageService.CreateMessage(user, messageDto.RoomId, messageDto.Text);
                    attachments = new List<FileModel>();
                }
                else
                {
                    var attachmentModels = await _fileService.GetAll(messageDto.Attachments);
                    attachments = attachmentModels.ToList();
                    message = await _messageService.CreateMessage(user, messageDto.RoomId, messageDto.Text, attachmentModels);
                }
                await SendMessage(message.Value, attachments, messageDto.Stamp);
            }
        }

        private void Disconnect() => Context.GetHttpContext().Abort();


        private async Task SendMessage(Message message, List<FileModel> attachments, Guid stamp)
        {
            var dto = Dto.CreateMessage(message, attachments);
            var ack = Dto.CreateMessageAck(message, stamp);
            await Task.WhenAll(
                Clients.OthersInGroup($"R{message.RoomId}").SendAsync("NewMessage", dto),
                Clients.Caller.SendAsync("MessageAck", ack)
                );
        }
    }
}
