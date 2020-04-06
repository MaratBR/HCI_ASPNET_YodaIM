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
        public Task<Result<Message>> CreateMessage(User sender, Guid roomId, string text, ICollection<FileModel> attachments = null);

        public Task<List<Message>> GetMessages(Guid roomId, int limit = 50, DateTime? beforeUtc = null);
    }
}
