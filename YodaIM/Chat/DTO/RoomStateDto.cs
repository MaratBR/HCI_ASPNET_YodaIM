using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class RoomStateDto
    {
        public Guid RoomId { get; set; }

        public List<Guid> Users { get; set; }
    }
}
