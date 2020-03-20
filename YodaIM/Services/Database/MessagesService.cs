using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;
using YodaIM.Services.Database;

namespace YodaIM.Services.Database
{
    public class MessagesService : DatabaseServiceBase, IMessagesService
    {
        public MessagesService(Context context) : base(context)
        {
        }

        public async Task<Message> CreateMessage(string text, Room room, User sender)
        {
            var message = new Message
            {
                Sender = sender,
                Room = room,
                Text = text
            };

            return await SaveMessage(message);
        }

        public async Task<Message> CreateMessage(string text, User receiver, User sender)
        {
            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Text = text
            };

            return await SaveMessage(message);
        }

        public async Task<Message> SaveMessage(Message message)
        {
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
