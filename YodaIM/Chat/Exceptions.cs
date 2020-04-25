using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Chat
{

    [Serializable]
    public class ChatRoomNotFoundException : Exception
    {
        public ChatRoomNotFoundException() { }
        public ChatRoomNotFoundException(string message) : base(message) { }
        public ChatRoomNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ChatRoomNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
