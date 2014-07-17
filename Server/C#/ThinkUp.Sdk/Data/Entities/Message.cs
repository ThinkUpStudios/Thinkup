using System;
using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Data.Entities
{
    public class Message : IMessage
    {
        public string Sender { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}
