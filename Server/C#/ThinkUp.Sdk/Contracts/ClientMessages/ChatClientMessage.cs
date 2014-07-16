﻿namespace ThinkUp.Sdk.Contracts.ClientMessages
{
    public class ChatClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public string ToUserName { get; set; }

        public string Message { get; set; }

        public bool IsGroupChat { get; set; }
    }
}
