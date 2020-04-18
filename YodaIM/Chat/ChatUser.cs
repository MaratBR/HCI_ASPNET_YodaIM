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
        private int connectionsCount;

        public User InnerUser { get; set; }

        public int ConnectionsCount => connectionsCount;

        // https://stackoverflow.com/questions/13181740/c-sharp-thread-safe-fastest-counter

        [Obsolete]
        public void IncrementConnections() => Interlocked.Increment(ref connectionsCount);

        [Obsolete]
        public void DecrementConnections() => Interlocked.Decrement(ref connectionsCount);
    }
}
