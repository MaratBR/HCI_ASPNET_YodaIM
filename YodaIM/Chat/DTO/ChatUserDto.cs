using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    public class ChatUserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte? Gender { get; set; }
    }
}
