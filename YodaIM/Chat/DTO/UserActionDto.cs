using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public enum UserActionType : byte
    {
        [Obsolete] Joined,
        [Obsolete] Left,
        QuacksterAscending
    }

    public class UserActionDto
    {
        public UserActionType ActionType { get; set; }

        public int UserId { get; set; }

        public Guid RoomId { get; set; }
    }
}
