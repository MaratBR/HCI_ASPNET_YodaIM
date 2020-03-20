using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;
using YodaIM.Services;

namespace YodaIM.Chat
{
    public static class ChatExtensions
    {
        public static async Task<Message> CreateMessageToSelf(this IMessagesService service, string text, User user) => await service.CreateMessage(text, user, user);
    }
}
