using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat.DTO
{
    [Obsolete]
    public class OldChatMessageDto
    {
        private readonly Message message;

        public long Id => message.Id;
        public string Text => message.Text;
        public DateTime PublishedAt => message.PublishedAt;
        public ICollection<Guid> Attachments => message.MessageAttachments?.Select(a => a.FileModelId).ToList() ?? new List<Guid>();
        public Guid SenderId => message.SenderId;
        public Guid RoomId => message.RoomId;

        public OldChatMessageDto(Message message)
        {
            this.message = message;
        }
    }
}
