using System.Collections.Generic;

namespace ThinkUp.Sdk.Interfaces
{
    public interface IConversation
    {
        string Name { get; }

        IEnumerable<string> GetParticipants();

        IEnumerable<IMessage> GetMessages();
    }
}
