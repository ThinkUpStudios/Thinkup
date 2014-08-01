using System.Collections.Generic;

namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ConnectedUsersListServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("There is a total of {0} users connected", this.ConectedUsersCount);
            }
        }

        public string UserName { get; set; }

        public IEnumerable<string> ConnectedUserNames { get; set; }

        public int ConectedUsersCount { get; set; }
    }
}
