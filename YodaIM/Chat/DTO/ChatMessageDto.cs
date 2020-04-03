using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class ChatMessageDto
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public List<Guid> Attachments { get; set; }

        public Guid SenderId { get; set; }

        public Guid RoomId { get; set; }

        public DateTime PublishedAt { get; set; }
    }
}
