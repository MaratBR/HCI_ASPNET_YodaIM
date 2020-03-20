using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    public interface IMessagesService
    {
        public Task<Message> CreateMessage(string text, Room room, User sender);

        public Task<Message> CreateMessage(string text, User receiver, User sender);
    }
}
