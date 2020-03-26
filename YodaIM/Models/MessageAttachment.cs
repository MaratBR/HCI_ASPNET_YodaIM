using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class MessageAttachment
    {
        public long MessageId { get; set; }

        public Guid FileModelId { get; set; }

        public virtual FileModel FileModel { get; set; }
        public virtual Message Message { get; set; }
    }
}
