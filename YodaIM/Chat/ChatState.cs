using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat
{
    public class ChatState : IChatState
    {
        private readonly Dictionary<string, ChatUser> users = new Dictionary<string, ChatUser>();

        public ChatUser AddConnection(string connectionId, User user)
        {
            users[connectionId] = new ChatUser { Id = user.Id };

            return users[connectionId];
        }

        public ChatUser GetUser(string connection)
        {
            return users[connection];
        }

        public bool IsOnline(User user)
        {
            return users.Any(p => p.Value.Id == user.Id);
        }

        public void RemoveConnection(string connectionId)
        {
            users.Remove(connectionId);
        }
    }
}
