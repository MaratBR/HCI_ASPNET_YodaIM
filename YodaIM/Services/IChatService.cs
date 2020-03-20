using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Services
{
    public interface IChatService
    {
        public Task<bool> RoomExists(int id);


    }
}
