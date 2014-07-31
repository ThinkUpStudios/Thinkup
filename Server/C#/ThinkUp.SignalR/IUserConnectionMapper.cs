using System.Collections.Generic;

namespace ThinkUp.SignalR
{
    public interface IUserConnectionMapper
    {
        int UsersCount { get; }

        IEnumerable<string> GetConnections(string userName);

        void AddConnection(string userName, string connectionId);

        void RemoveConnection(string userName, string connectionId);
    }
}
