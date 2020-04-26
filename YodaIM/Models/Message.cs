using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Text { get; set; }

        public int SenderId { get; set; }
        public Guid RoomId { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual User Sender { get; set; }

        [JsonIgnore]
        public virtual Room Room { get; set; }

        [JsonIgnore]
        public virtual List<MessageAttachment> MessageAttachments { get; set; }
    }
}
