using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat.DTO
{
    public class UserDepartedDto
    {
        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }
    }
}
