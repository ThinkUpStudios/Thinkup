using System;

namespace ThinkUp.Sdk.Interfaces
{
    public interface IMessage
    {
        string Sender { get; }

        string Content { get; }

        DateTime Date { get; }
    }
}
