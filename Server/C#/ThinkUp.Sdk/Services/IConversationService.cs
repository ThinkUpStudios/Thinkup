using System.Collections.Generic;
using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Services
{
    public interface IConversationService
    {
        IEnumerable<IConversation> GetAllByParticipant(string userName);

        IConversation GetByName(string conversationName);

        IEnumerable<IMessage> GetMessages(string conversationName);

        bool Exist(string conversationName);

        ///<exception cref="ServiceException">ServiceException</exception>
        void CreateConversation(params string[] userNames);

        ///<exception cref="ServiceException">ServiceException</exception>
        void CreateConversation(string conversationName, params string[] userNames);

        ///<exception cref="ServiceException">ServiceException</exception>
        void AddMessage(string conversationName, string userName, string message);

        ///<exception cref="ServiceException">ServiceException</exception>
        void AddParticipant(string conversationName, string userName);

        ///<exception cref="ServiceException">ServiceException</exception>
        void DeleteConversation(string conversationName);
    }
}
