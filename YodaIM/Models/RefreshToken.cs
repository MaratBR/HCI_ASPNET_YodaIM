using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public class RefreshToken
    {
        public RefreshToken()
        {
        }

        public RefreshToken(User user, TimeSpan lifeTime)
        {
            User = user;
            Expires = CreatedAt.Add(lifeTime);
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime Expires { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
