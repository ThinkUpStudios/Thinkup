using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Data.Entities;

namespace ThinkUp.Sdk.Tests.DataTests
{
    [TestClass]
    public class ConversationRepositoryTests : RepositoryTests<Conversation>
    {
        [TestMethod]
        public void UT_When_CreateConversation_Then_Success()
        {
            var user1Name = GetUniqueName("user1");
            var user2Name = GetUniqueName("user2");
            var message1 = new Message
            {
                Sender = user1Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message2 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversationName = GetUniqueName("Conversation");
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { user1Name, user2Name },
                Messages = new List<Message> { message1, message2 },
            };

            this.testRepository.Create(conversation);

            var createdConversation = this.testRepository.Get(e => e.Name == conversationName);

            Assert.IsNotNull(createdConversation);
            Assert.AreEqual(conversation.Id, createdConversation.Id);
            Assert.AreEqual(conversationName, createdConversation.Name);
            Assert.AreEqual(conversation.Messages.Count, createdConversation.Messages.Count);
            Assert.AreEqual(conversation.Participants.Count, createdConversation.Participants.Count);
        }

        [TestMethod]
        public void UT_When_UpdateConversation_Then_Success()
        {
            var user1Name = GetUniqueName("user1");
            var user2Name = GetUniqueName("user2");
            var message1 = new Message
            {
                Sender = user1Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message2 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversationName = GetUniqueName("Conversation");
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { user1Name, user2Name },
                Messages = new List<Message> { message1, message2 },
            };

            this.testRepository.Create(conversation);

            var createdConversation = this.testRepository.Get(e => e.Name == conversationName);

            var user3Name = GetUniqueName("user3");
            var message3 = new Message
            {
                Sender = user3Name,
                Date = DateTime.Now,
                Content = "Baz Message"
            };

            createdConversation.Participants.Add(user3Name);
            createdConversation.Messages.Add(message3);

            this.testRepository.Update(createdConversation);

            var updatedConversation = this.testRepository.Get(e => e.Name == conversationName);

            Assert.IsNotNull(updatedConversation);
            Assert.AreEqual(createdConversation.Id, updatedConversation.Id);
            Assert.AreEqual(createdConversation.Name, updatedConversation.Name);
            Assert.AreEqual(createdConversation.Participants.Count, updatedConversation.Participants.Count);
            Assert.AreEqual(createdConversation.Messages.Count, updatedConversation.Messages.Count);
        }

        [TestMethod]
        public void UT_When_DeleteConversation_Then_Success()
        {
            var user1Name = GetUniqueName("user1");
            var user2Name = GetUniqueName("user2");
            var message1 = new Message
            {
                Sender = user1Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message2 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversationName = GetUniqueName("Conversation");
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { user1Name, user2Name },
                Messages = new List<Message> { message1, message2 },
            };

            this.testRepository.Create(conversation);

            var createdConversation = this.testRepository.Get(e => e.Name == conversationName);

            this.testRepository.Delete(createdConversation);

            var deletedConversation = this.testRepository.Get(e => e.Name == conversationName);

            Assert.IsNull(deletedConversation);
        }

        [TestMethod]
        public void UT_When_DeleteAllConversations_Then_Success()
        {
            var user1Name = GetUniqueName("user1");
            var user2Name = GetUniqueName("user2");
            var message1 = new Message
            {
                Sender = user1Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message2 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversation1Name = GetUniqueName("Conversation");
            var conversation1 = new Conversation
            {
                Name = conversation1Name,
                Participants = new List<string> { user1Name, user2Name },
                Messages = new List<Message> { message1, message2 },
            };

            var user3Name = GetUniqueName("user1");
            var message3 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message4 = new Message
            {
                Sender = user3Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversation2Name = GetUniqueName("Conversation");
            var conversation2 = new Conversation
            {
                Name = conversation2Name,
                Participants = new List<string> { user2Name, user3Name },
                Messages = new List<Message> { message3, message4 },
            };

            this.testRepository.Create(conversation1);
            this.testRepository.Create(conversation2);

            var currentConversationsCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var existingConversations = this.testRepository.GetAll();

            Assert.AreEqual(2, currentConversationsCount);
            Assert.AreEqual(0, existingConversations.Count());
        }

        [TestMethod]
        public void UT_When_GetConversationWithPredicate_Then_Success()
        {
            var user1Name = GetUniqueName("user1");
            var user2Name = GetUniqueName("user2");
            var message1 = new Message
            {
                Sender = user1Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message2 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversation1Name = GetUniqueName("Conversation");
            var conversation1 = new Conversation
            {
                Name = conversation1Name,
                Participants = new List<string> { user1Name, user2Name },
                Messages = new List<Message> { message1, message2 },
            };

            var user3Name = GetUniqueName("user1");
            var message3 = new Message
            {
                Sender = user2Name,
                Date = DateTime.Now,
                Content = "Foo Message"
            };
            var message4 = new Message
            {
                Sender = user3Name,
                Date = DateTime.Now,
                Content = "Bar Message"
            };
            var conversation2Name = GetUniqueName("Conversation");
            var conversation2 = new Conversation
            {
                Name = conversation2Name,
                Participants = new List<string> { user2Name, user3Name },
                Messages = new List<Message> { message3, message4 },
            };

            this.testRepository.Create(conversation1);
            this.testRepository.Create(conversation2);

            var testConversations = this.testRepository.GetAll(e => e.Participants.Contains(user1Name));

            Assert.AreEqual(1, testConversations.Count());
        }
    }
}
