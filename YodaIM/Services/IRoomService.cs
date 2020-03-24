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

        Task<ICollection<Room>> ListRooms(User user);

        Task<bool> Exists(Guid id);
    }
}
