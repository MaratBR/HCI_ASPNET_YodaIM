using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class UserRoom
    {
        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }

        public DateTime Since { get; set; } = DateTime.UtcNow;

        public User User { get; set; }

        public Room Room { get; set; }
    }
}
