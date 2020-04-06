using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    public class ChatMessageSenderDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
    }
}
