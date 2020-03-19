using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class RoomUsers
    {

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
    }
}
