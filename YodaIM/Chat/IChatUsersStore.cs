using FluentResults;
using System.Collections.Generic;
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

        ICollection<int> GetRoomIds([NotNull] string connectionId);

        bool InRoom([NotNull] string connectionId, int roomId);

        Result AddRoom([NotNull] string connectionId, int roomId);

        Result RemoveRoom([NotNull] string connectionId, int roomId);
    }

    class ChatUser
    {
        private int connectionsCount;

        public User User { get; set; }

        public int ConnectionsCount => connectionsCount;

        // https://stackoverflow.com/questions/13181740/c-sharp-thread-safe-fastest-counter

        public void IncrementConnections() => Interlocked.Increment(ref connectionsCount);

        public void DecrementConnections() => Interlocked.Decrement(ref connectionsCount);
    }

    class ChatHandler : IChatHandler
    {
        private Dictionary<string, int> connectionUsers = new Dictionary<string, int>();
        private Dictionary<string, HashSet<int>> connectionRooms = new Dictionary<string, HashSet<int>>();
        private Dictionary<int, ChatUser> users = new Dictionary<int, ChatUser>();
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
                        User = user
                    };
                }
            }
            connectionUsers[connectionId] = user.Id;
            users[user.Id].IncrementConnections();
            connectionRooms[connectionId] = new HashSet<int>();

            return Results.Ok();
        }

        public Result AddRoom([NotNull] string connectionId, int roomId)
        {
            if (connectionRooms.ContainsKey(connectionId))
            {
                connectionRooms[connectionId].Add(roomId);
                return Results.Ok();
            }
            return Results.Fail("Connection " + connectionId + " not found");
        }

        public ICollection<int> GetRoomIds([NotNull] string connectionId)
        {
            if (connectionRooms.ContainsKey(connectionId))
                return connectionRooms[connectionId];

            throw new KeyNotFoundException("No rooms found for connection ID = " + connectionId);
        }

        public bool InRoom([NotNull] string connectionId, int roomId) => connectionRooms.ContainsKey(connectionId) && connectionRooms[connectionId].Contains(roomId);

        public Result RemoveConnection([NotNull] string connectionId)
        {
            if (!connectionUsers.ContainsKey(connectionId))
                return Results.Fail("Connection not found");
            int userId = connectionUsers[connectionId];
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

        public Result RemoveRoom([NotNull] string connectionId, int roomId)
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

            int id = connectionUsers[connectionId];

            return users[id].User;
        }
    }
}