using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    public interface IMessageService
    {
        public Task<Result<Message>> CreateMessage(User sender, int roomId, string text);

        public Task<Result<Message>> CreateMessage(User sender, int roomId, FileModel file);

    }
}
