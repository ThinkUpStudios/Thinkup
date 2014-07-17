using System.Collections.Generic;
using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Data.Entities
{
    public class Conversation : DataEntity, IConversation
    {
        public string Name { get; set; }

        public List<string> Participants { get; set; }

        public List<Message> Messages { get; set; }

        public Conversation()
        {
            this.Participants = new List<string>();
            this.Messages = new List<Message>();
        }

        public IEnumerable<string> GetParticipants()
        {
            return this.Participants;
        }

        public IEnumerable<IMessage> GetMessages()
        {
            return this.Messages;
        }
    }
}
