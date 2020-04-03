using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public enum UserActionType : byte
    {
        Joined,
        Left
    }

    public class UserActionDto
    {
        public UserActionType ActionType { get; set; }

        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }
    }
}
