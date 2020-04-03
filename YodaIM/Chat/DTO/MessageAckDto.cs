using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class MessageAckDto
    {
        public Guid Stamp { get; set; }

        public long Id { get; set; }
    }
}
