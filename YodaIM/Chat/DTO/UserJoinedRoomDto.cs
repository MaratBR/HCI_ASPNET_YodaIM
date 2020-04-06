using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class UserJoinedRoomDto
    {
        public ChatUserDto User { get; set; }

        public Guid RoomId { get; set; }
    }
}
