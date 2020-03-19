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

        public int SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public int RoomId { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public virtual Room Room { get; set; }
    }
}
