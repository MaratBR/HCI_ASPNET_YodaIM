using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat
{
    public class ChatUser
    {
        private int connectionsCount = 0;

        public Guid Id { get; set; }

        public ICollection<Guid> Rooms { get; } = new HashSet<Guid>();

        public int ConnectionsCount => connectionsCount;

        // https://stackoverflow.com/questions/13181740/c-sharp-thread-safe-fastest-counter

        public void IncrementConnections() => Interlocked.Increment(ref connectionsCount);

        public void DecrementConnections() => Interlocked.Decrement(ref connectionsCount);
    }
}
