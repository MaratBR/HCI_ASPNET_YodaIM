using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public virtual User Founder { get; set; }
    }
}
