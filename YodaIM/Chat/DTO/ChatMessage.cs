using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    public class ChatMessage
    {
        private readonly Message message;

        public long Id => message.Id;
        public string Text => message.Text;
        public DateTime PublishedAt => message.PublishedAt;
        public IEnumerable<Guid> Attachments => message.MessageAttachments.Select(a => a.FileModelId);
        public Guid SenderId => message.SenderId;
        public Guid RoomId => message.RoomId;

        public ChatMessage(Message message)
        {
            this.message = message;
        }
    }
}
