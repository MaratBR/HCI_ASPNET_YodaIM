using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class BinaryBlob
    {
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public byte[] Sha256 { get; set; }
    }
}
