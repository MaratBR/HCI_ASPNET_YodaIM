using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class UserRoom
    {
        public Guid RoomId { get; set; }

        public Guid UserId { get; set; }

        public DateTime Since { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Room Room { get; set; }
    }
}
