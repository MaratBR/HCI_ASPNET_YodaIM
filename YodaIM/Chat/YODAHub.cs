using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Chat
{
    [Authorize]
    public class YODAHub : Hub
    {
        private readonly IChatHandler _chatHandler;
        private readonly IRoomService _roomService;
        private readonly IMessageService _messageService;
        private readonly UserManager<User> _userManager;

        public YODAHub(IChatHandler chatHandler,
            IRoomService roomService,
            IMessageService messageService,
            UserManager<User> userManager)
        {
            _chatHandler = chatHandler;
            _roomService = roomService;
            _userManager = userManager;
            _messageService = messageService;
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

            var result = _chatHandler.AddConnection(Context.ConnectionId, user);
            if (result.IsFailed)
            {
                Disconnect();
                return;
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var rooms = _chatHandler.GetRoomIds(Context.ConnectionId);
            var user = _chatHandler.User(Context.ConnectionId);

            await Task.WhenAll(
                rooms.Select(roomId => SendUserLeftMessage(user.Id, roomId)).ToArray()
                );

            _chatHandler.RemoveConnection(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Send(string text, Guid roomId)
        {
            if (_chatHandler.InRoom(Context.ConnectionId, roomId))
            {
                var user = _chatHandler.User(Context.ConnectionId);
                var message = await _messageService.CreateMessage(user, roomId, text);
                await SendMessage(message.Value);
            }
        }

        public async Task JoinRoom(Guid roomId)
        {
            if (_chatHandler.InRoom(Context.ConnectionId, roomId))
            {
                return;
            }

            if (await _roomService.Exists(roomId))
            {
                var user = _chatHandler.User(Context.ConnectionId);
                await Groups.AddToGroupAsync(Context.ConnectionId, RoomGroup(roomId));
                await SendUserJoinedMessage(user.Id, roomId);
                _chatHandler.AddRoom(Context.ConnectionId, roomId);
            }
        }

        public async Task LeaveRoom(Guid roomId)
        {
            if (!_chatHandler.InRoom(Context.ConnectionId, roomId))
            {
                return;
            }

            var user = _chatHandler.User(Context.ConnectionId);
            await SendUserLeftMessage(user.Id, roomId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, RoomGroup(roomId));
            _chatHandler.RemoveRoom(Context.ConnectionId, roomId);
        }

        private void Disconnect() => Context.GetHttpContext().Abort();

        private async Task SendUserJoinedMessage(Guid userId, Guid roomId) => await Clients.Group(RoomGroup(roomId)).SendAsync("Joined", userId, roomId);

        private async Task SendUserLeftMessage(Guid userId, Guid roomId) => await Clients.Group(RoomGroup(roomId)).SendAsync("Left", userId, roomId);

        private async Task SendMessage(Message message) => await Clients.Group(RoomGroup(message.RoomId)).SendAsync("Message", message);

        private string RoomGroup(Guid id) => id.ToString();
    }
}
