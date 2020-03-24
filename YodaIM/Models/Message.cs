using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Text { get; set; }

        public Guid SenderId { get; set; }
        public Guid RoomId { get; set; }

        public Guid? FileId { get; set; }

        public virtual User Sender { get; set; }
        public virtual Room Room { get; set; }
        public virtual FileModel File { get; set; }
    }
}
