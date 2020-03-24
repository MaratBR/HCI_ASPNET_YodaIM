using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services.Database
{
    public class RoomService : DatabaseServiceBase, IRoomService
    {
        public RoomService(Context context) : base(context)
        {
        }

        public async Task<Room> CreateRoom(IRoomInfo info)
        {
            var room = new Room
            {
                Name = info.Name,
                Description = info.Description
            };

            context.Rooms.Add(room);
            await context.SaveChangesAsync();
            return room;
        }

        public Task<bool> Exists(Guid id) => context.Rooms.Where(r => r.Id == id).AnyAsync();

        public Task<Room> GetById(Guid id) => context.Rooms.Where(r => r.Id == id).SingleOrDefaultAsync();

        public async Task<ICollection<Room>> ListRooms(User user) => await context.Rooms.ToListAsync();
    }
}
