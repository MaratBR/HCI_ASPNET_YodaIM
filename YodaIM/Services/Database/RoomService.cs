using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services.Database
{
    public class RoomService : DatabaseServiceBase, IRoomsService
    {
        public RoomService(Context context) : base(context)
        {
        }

        public async Task<Room> GetById(int id) => await context.Rooms.Where(r => r.Id == id).SingleOrDefaultAsync();

        public async Task<List<Room>> ListRooms(User user) => await context.Rooms.ToListAsync();
    }
}
