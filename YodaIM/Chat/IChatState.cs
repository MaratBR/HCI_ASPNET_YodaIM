using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat
{
    public interface IChatState
    {
        ChatUser GetUser(string connection);

        ChatUser AddConnection(string connectionId, User user);

        void RemoveConnection(string connectionId);

        bool IsOnline(User user);
    }
}
