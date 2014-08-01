using System;
using System.Collections.Generic;
using System.Linq;

namespace ThinkUp.Sdk.Contracts.ServerMessages
{
    public class ConversationDetailServerMessage : IServerMessage
    {
        private List<string> participants;
        private List<ChatMessage> messages;

        public string Message
        {
            get
            {
                return string.Format("Conversation {0}: {1} participants, {2} messages", this.ConversationName, this.Participants.Count(), this.Messages.Count());
            }
        }

        public string ConversationName { get; set; }

        public IEnumerable<string> Participants { get { return this.participants; } }

        public IEnumerable<ChatMessage> Messages { get { return this.messages; } }

        public ConversationDetailServerMessage()
        {
            this.participants = new List<string>();
            this.messages = new List<ChatMessage>();
        }

        public void AddParticipant(string participant)
        {
            this.participants.Add(participant);
        }

        public void AddMessage(ChatMessage message)
        {
            this.messages.Add(message);
        }
    }

    public class ChatMessage
    {
        public string Sender { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
