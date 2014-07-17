using System;
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Data;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IRepository<Conversation> conversationRepository;

        public ConversationService(IRepository<Conversation> conversationRepository)
        {
            this.conversationRepository = conversationRepository;
        }

        public IEnumerable<IConversation> GetAllByUser(string userName)
        {
            return this.conversationRepository.GetAll(c => c.Participants.Contains(userName));
        }

        public IConversation GetByName(string conversationName)
        {
            return this.conversationRepository.Get(c => c.Name == conversationName);
        }

        public IEnumerable<IMessage> GetMessages(string conversationName)
        {
            return this.GetByName(conversationName).GetMessages();
        }

        public bool Exist(string conversationName)
        {
            return this.conversationRepository.Exist(c => c.Name == conversationName);
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void CreateConversation(params string[] userNames)
        {
            var conversationName = string.Join(" & ", userNames.Sort());

            this.CreateConversation(conversationName, userNames);
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void CreateConversation(string conversationName, params string[] userNames)
        {
            if (userNames.Length < 2)
            {
                var errorMessage = string.Format("A conversation must have at least two participants. {0} participant is not allowed", userNames.Length);

                throw new ServiceException(errorMessage);
            }

            if (this.Exist(conversationName))
            {
                return;
            }

            var conversation = new Conversation
            {
                Name = conversationName
            };

            conversation.Participants.AddRange(userNames);

            try
            {
                this.conversationRepository.Create(conversation);
            }
            catch (DataException dataEx)
            {
                var errorMessage = string.Format("An error occured when creating the conversation {0}", conversationName);

                throw new ServiceException(errorMessage, dataEx);
            }
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void AddMessage(string conversationName, string userName, string message)
        {
            var existingConversation = this.conversationRepository.Get(c => c.Name == conversationName);

            if (existingConversation == null)
            {
                var errorMessage = string.Format("The conversation {0} does not exist", conversationName);

                throw new ServiceException(errorMessage);
            }

            existingConversation.Messages.Add(new Message
            {
                Sender = userName,
                Content = message,
                Date = DateTime.Now
            });

            try
            {
                this.conversationRepository.Update(existingConversation);
            }
            catch (DataException dataEx)
            {
                var errorMessage = string.Format("An error occured when updating the conversation {0}", conversationName);

                throw new ServiceException(errorMessage, dataEx);
            }
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void AddUser(string conversationName, string userName)
        {
            var existingConversation = this.conversationRepository.Get(c => c.Name == conversationName);

            if (existingConversation == null)
            {
                var errorMessage = string.Format("The conversation {0} does not exist", conversationName);

                throw new ServiceException(errorMessage);
            }

            if (existingConversation.Participants.Contains(userName))
            {
                var errorMessage = string.Format("The user {0} is already at the conversation {1}", userName, conversationName);

                throw new ServiceException(errorMessage);
            }

            existingConversation.Participants.Add(userName);

            try
            {
                this.conversationRepository.Update(existingConversation);
            }
            catch (DataException dataEx)
            {
                var errorMessage = string.Format("An error occured when updating the conversation {0}", conversationName);

                throw new ServiceException(errorMessage, dataEx);
            }
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void DeleteConversation(string conversationName)
        {
            var existingConversation = this.conversationRepository.Get(c => c.Name == conversationName);

            try
            {
                this.conversationRepository.Delete(existingConversation);
            }
            catch (DataException dataEx)
            {
                var errorMessage = string.Format("An error occured when deleting the conversation {0}", conversationName);

                throw new ServiceException(errorMessage, dataEx);
            }
        }
    }
}
