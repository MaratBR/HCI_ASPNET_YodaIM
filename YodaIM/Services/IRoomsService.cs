using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    public interface IRoomsService
    {
        Task<Room> GetById(int id);
        Task<List<Room>> ListRooms(User user);
    }
}
