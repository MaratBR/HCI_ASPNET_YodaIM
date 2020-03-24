using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public enum FileType
    {
        Generic,
        Avatar
    };

    [Table("File")]
    public class FileModel
    {
        public int Id { get; set; }

        [MaxLength(255)] [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public FileType Type { get; set; }

        [Required] [MaxLength(1024 * 1024 * 10)]
        public byte[] Data { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
