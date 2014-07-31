using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ThinkUp.SignalR
{
    public class UserConnectionMapper : IUserConnectionMapper
    {
        private static readonly ConcurrentDictionary<string, IList<string>> userConnections;

        public int UsersCount { get { return userConnections.Count; } }

        static UserConnectionMapper()
        {
            userConnections = new ConcurrentDictionary<string, IList<string>>();
        }

        public UserConnectionMapper()
        {
        }

        public IEnumerable<string> GetConnections(string userName)
        {
            var connectionIds = default(IList<string>);

            if (!userConnections.TryGetValue(userName, out connectionIds))
            {
                connectionIds = new List<string>();
            }

            return connectionIds;
        }

        public void AddConnection(string userName, string connectionId)
        {
            var connectionIds = default(IList<string>);

            if (!userConnections.TryGetValue(userName, out connectionIds))
            {
                connectionIds = new List<string>();
                userConnections.TryAdd(userName, connectionIds);
            }

            connectionIds.Add(connectionId);
        }

        public void RemoveConnection(string userName, string connectionId)
        {
            var connectionIds = default(IList<string>);

            if (!userConnections.TryGetValue(userName, out connectionIds))
            {
                return;
            }

            connectionIds.Remove(connectionId);

            if (connectionIds.Count == 0)
            {
                userConnections.TryRemove(userName, out connectionIds);
            }
        }
    }
}