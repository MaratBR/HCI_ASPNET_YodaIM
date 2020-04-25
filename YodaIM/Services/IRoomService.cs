using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    public interface IRoomInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public interface IRoomService
    {
        Task<Room> GetById(Guid id);

        Task<Room> CreateRoom(IRoomInfo info);

        Task<List<Room>> ListRooms(User user);

        Task<bool> Exists(Guid id);
        Task<bool> InRoom(User user, Room room);

        Task<bool> InRoom(User user, Guid roomId);

        Task JoinRoom(User user, Room room);

        Task LeaveRoom(User user, Room room);

        Task<List<UserRoom>> GetUsersFromRoom(Room room);
    }
}
