﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class ChatMessageRequestDto
    {
        public Guid RoomId { get; set; }

        public string Text { get; set; }

        public Guid Stamp { get; set; }

        public List<Guid> Attachments { get; set; }
    }
}
