using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat
{
    public class ChatRoom
    {
        public Room RoomModel { get; set; }

        public ICollection<ChatUser> Users { get; }

        public ChatRoom(Room room)
        {
            RoomModel = room;
            Users = new HashSet<ChatUser>();
        }
    }
}
