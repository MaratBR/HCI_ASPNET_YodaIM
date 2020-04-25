using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    public class ChatMembershipDto
    {
        public User User { get; set; }

        public bool IsOnline { get; set; }
    }
}
