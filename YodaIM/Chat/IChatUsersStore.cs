using FluentResults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Chat
{
    public interface IChatHandler
    {
        Result AddConnection([NotNull] string connectionId, [NotNull] User user);

        Result RemoveConnection([NotNull] string connectionId);

        User User([NotNull] string connectionId);

        List<User> GetUsersFromRoom(Guid roomId);

        ICollection<Guid> GetRoomIds([NotNull] string connectionId);

        bool InRoom([NotNull] string connectionId, Guid roomId);

        Result AddRoom([NotNull] string connectionId, Guid roomId);

        Result RemoveRoom([NotNull] string connectionId, Guid roomId);
    }

    class ChatHandler : IChatHandler
    {
        private Dictionary<string, Guid> connectionUsers = new Dictionary<string, Guid>();
        private Dictionary<string, HashSet<Guid>> connectionRooms = new Dictionary<string, HashSet<Guid>>();
        private Dictionary<Guid, ChatUser> users = new Dictionary<Guid, ChatUser>();
        private object _lock = new object();

        public Result AddConnection([NotNull] string connectionId, [NotNull] User user)
        {
            if (connectionUsers.ContainsKey(connectionId))
                return Results.Fail("This connection ID is already present");

            lock (_lock)
            {
                if (!users.ContainsKey(user.Id))
                {
                    users[user.Id] = new ChatUser
                    {
                        InnerUser = user
                    };
                }
            }
            connectionUsers[connectionId] = user.Id;
            users[user.Id].IncrementConnections();
            connectionRooms[connectionId] = new HashSet<Guid>();

            return Results.Ok();
        }

        public Result AddRoom([NotNull] string connectionId, Guid roomId)
        {
            if (connectionRooms.ContainsKey(connectionId))
            {
                connectionRooms[connectionId].Add(roomId);
                return Results.Ok();
            }
            return Results.Fail("Connection " + connectionId + " not found");
        }

        public ICollection<Guid> GetRoomIds([NotNull] string connectionId)
        {
            if (connectionRooms.ContainsKey(connectionId))
                return connectionRooms[connectionId];

            throw new KeyNotFoundException("No rooms found for connection ID = " + connectionId);
        }

        public List<User> GetUsersFromRoom(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public bool InRoom([NotNull] string connectionId, Guid roomId) => connectionRooms.ContainsKey(connectionId) && connectionRooms[connectionId].Contains(roomId);

        public Result RemoveConnection([NotNull] string connectionId)
        {
            if (!connectionUsers.ContainsKey(connectionId))
                return Results.Fail("Connection not found");
            Guid userId = connectionUsers[connectionId];
            connectionUsers.Remove(connectionId);
            connectionRooms.Remove(connectionId);

            users[userId].DecrementConnections();
            lock (_lock)
            {
                if (users[userId].ConnectionsCount == 0)
                {
                    users.Remove(userId);
                }
            }

            return Results.Ok();
        }

        public Result RemoveRoom([NotNull] string connectionId, Guid roomId)
        {
            if (connectionRooms.ContainsKey(connectionId))
            {
                connectionRooms[connectionId].Remove(roomId);
                return Results.Ok();
            }
            return Results.Fail("Connection " + connectionId + " not found");
        }

        public User User([NotNull] string connectionId)
        {
            if (!connectionUsers.ContainsKey(connectionId))
                return null;

            Guid id = connectionUsers[connectionId];

            return users[id].InnerUser;
        }
    }
}