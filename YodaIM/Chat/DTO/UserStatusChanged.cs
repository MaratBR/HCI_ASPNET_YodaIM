using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class UserStatusChanged
    {
        public Guid UserId { get; set; }

        public bool IsOnline { get; set; }
    }
}
