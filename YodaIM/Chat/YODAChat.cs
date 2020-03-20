using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Helpers;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Chat
{
    class UserInfo
    {
        public User User { get; set; }

        public HashSet<int> Rooms { get; private set; } = new HashSet<int>();
    }

    class ChatMessage
    {
        public int SenderId { get; set; }
        public int RoomId { get; set; }

        public string Text { get; set; }

        public List<int> Attachments { get; set; }
    }

    [Authorize]
    public class YODAChat : Hub
    {
        private readonly IChatService chatService;
        private readonly UserManager<User> userManager;
        private readonly Dictionary<string, UserInfo> users;

        public YODAChat(IChatService chatService, UserManager<User> userManager)
        {
            this.chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            try
            {
                users[Context.ConnectionId] = new UserInfo
                {
                    User = await userManager.GetUserAsyncOrFail(Context.User)
                };
            }
            catch(ApplicationException)
            {
                Context.GetHttpContext().Abort();
            }
        }

        public async Task Join(int roomId)
        {
            if (await chatService.RoomExists(roomId))
            {
                await JoinRoom(roomId);
            }
        }

        public async Task SendText(string text, int roomId)
        {

            if (InRoom(roomId))
            {
                var user = GetUser();
                await Clients.Group(GroupName(roomId)).SendAsync("Text",
                    new ChatMessage
                    {
                        SenderId = user.Id,
                        Text = text,
                        RoomId = roomId
                    });
            }
        }

        private async Task JoinRoom(int roomId)
        {
            var user = GetUser();
            string groupName = GroupName(roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Joined", user);
        }

        private string GroupName(int roomId)
        {
            return roomId.ToString();
        }

        private User GetUser() => GetUserInfo()?.User;

        private bool InRoom(int id) => GetUserInfo()?.Rooms?.Contains(id) == true;

        private UserInfo GetUserInfo()
        {
            return users.ContainsKey(Context.ConnectionId) ? users[Context.ConnectionId] : null;
        }
    }
}
