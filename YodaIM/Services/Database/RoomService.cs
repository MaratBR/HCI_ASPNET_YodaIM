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

        public Task<List<User>> GetUsersFromRoom(Room room)
        {
            return context.UserRooms
                .Where(ur => ur.RoomId == room.Id)
                .Select(ur => ur.User)
                .ToListAsync();
        }

        public Task<bool> InRoom(User user, Room room) => InRoom(user, room.Id);

        public Task<bool> InRoom(User user, Guid roomId)
        {
            return context.UserRooms.Where(
                ur => ur.UserId == user.Id && ur.RoomId == roomId
                ).AnyAsync();
        }

        public Task JoinRoom(User user, Room room)
        {
            context.UserRooms.Add(new UserRoom
            {
                UserId = user.Id,
                RoomId = room.Id
            });

            return context.SaveChangesAsync();
        }

        public async Task LeaveRoom(User user, Room room)
        {
            var assoc = await context.UserRooms.Where(ur => ur.RoomId == room.Id && ur.UserId == user.Id).SingleOrDefaultAsync();
            if (assoc != null)
                context.UserRooms.Remove(assoc);
        }

        public async Task<List<Room>> ListRooms(User user)
        {
            return await context.Rooms
                .Where(r => r.Users.Any(ur => ur.UserId == user.Id))
                .ToListAsync();
        }
    }
}
