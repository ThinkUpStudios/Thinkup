using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Interfaces;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Tests.PluginTests.PluginComponentTests
{
    [TestClass]
    public class ConversationsPluginComponentTests
    {
        private readonly string requestUser = "user1";

        private ISerializer serializer;
        private Mock<IConversationService> conversationServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IPluginComponent conversationsPluginComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.conversationServiceMock = new Mock<IConversationService>();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.conversationsPluginComponent = new ConversationsPluginComponent(this.conversationServiceMock.Object, this.notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void UT_When_HandleNewConversationWithoutName_Then_Success()
        {
            this.conversationServiceMock
                .Setup(s => s.CreateConversation(It.IsAny<string[]>()))
                .Verifiable();

            var newConversationClientMessage = new NewConversationClientMessage
            {
                UserName = this.requestUser,
                Participants = new List<string> { this.requestUser, "user2" }
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.NewConversation,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(newConversationClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock
                .Verify(s => s.CreateConversation(It.Is<string[]>(x => x.Length == 2 && x[0] == this.requestUser && x[1] == "user2")));

            Assert.IsTrue(canHandle);
        }

        public void UT_When_HandleNewConversationWithName_Then_Success()
        {
            this.conversationServiceMock
                .Setup(s => s.CreateConversation(It.IsAny<string>(), It.IsAny<string[]>()))
                .Verifiable();

            var conversationName = "Test Conversation";
            var newConversationClientMessage = new NewConversationClientMessage
            {
                UserName = this.requestUser,
                ConversationName = conversationName,
                Participants = new List<string> { this.requestUser, "user2" }
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.NewConversation,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(newConversationClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock
                .Verify(s => s.CreateConversation(It.Is<string>(x => x == conversationName), It.Is<string[]>(x => x.Length == 2 && x[0] == this.requestUser && x[1] == "user2")));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleNewConversationParticipant_Then_Success()
        {
            var conversationName = "Test Conversation";
            var newParticipantName = "user3";

            this.conversationServiceMock
                .Setup(s => s.AddParticipant(It.Is<string>(x => x == conversationName), It.Is<string>(x => x == newParticipantName)))
                .Verifiable();

            var newConversationParticipantClientMessage = new NewConversationParticipantClientMessage
            {
                ConversationName = conversationName,
                UserName = this.requestUser,
                NewParticipantName = newParticipantName
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.NewConversationParticipant,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(newConversationParticipantClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock.VerifyAll();

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleGetConversations_Then_Success()
        {
            var conversation1 = new Conversation 
            { 
                Name = "Conversation 1", 
                Participants = new List<string> { this.requestUser, "user2" }, 
                Messages = new List<Message> { 
                    new Message { 
                        Sender = "user2", 
                        Date = DateTime.Now, 
                        Content = "Message 1" 
                    }
                }
            };
            var conversation2 = new Conversation 
            { 
                Name = "Conversation 2", 
                Participants = new List<string> { "user3", "user4" }, 
                Messages = new List<Message> { 
                    new Message { 
                        Sender = "user3", 
                        Date = DateTime.Now, 
                        Content = "Message 1" 
                    }
                }
            };

            this.conversationServiceMock
                .Setup(s => s.GetAllByParticipant(It.Is<string>(x => x == this.requestUser)))
                .Returns(new List<IConversation> { conversation1, conversation2 })
                .Verifiable();

            var getConversationsClientMessage = new GetConversationsClientMessage
            {
                UserName = this.requestUser,
                PageSize = 50
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.GetConversations,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(getConversationsClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == ServerMessageType.ConversationsList),
                It.Is<object>(o => (((ConversationsListServerMessage)o).ConversationsCount == 2)),
                It.Is<string>(x => x == this.requestUser)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleGetConversation_Then_Success()
        {
            var conversationName = "Conversation 1";
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { this.requestUser, "user2" },
                Messages = new List<Message> { 
                    new Message { 
                        Sender = "user2", 
                        Date = DateTime.Now, 
                        Content = "Message 1" 
                    }
                }
            };

            this.conversationServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == conversationName)))
                .Returns(conversation)
                .Verifiable();

            var getConversationClientMessage = new GetConversationClientMessage
            {
                UserName = this.requestUser,
                ConversationName = conversationName
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.GetConversation,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(getConversationClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == ServerMessageType.ConversationDetail),
                It.Is<object>(o => (((ConversationDetailServerMessage)o).ConversationName == conversationName)),
                It.Is<string>(x => x == this.requestUser)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleChat_Then_Success()
        {
            var conversationName = "Conversation 1";
            var message = "Message 1";
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { this.requestUser, "user2" }
            };

            this.conversationServiceMock
                .Setup(s => s.AddMessage(It.Is<string>(x => x == conversationName), It.Is<string>(x => x == this.requestUser), It.Is<string>(x => x == message)))
                .Verifiable();
            this.conversationServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == conversationName)))
               .Returns(conversation)
               .Verifiable();

            var chatClientMessage = new ChatClientMessage
            {
                UserName = this.requestUser,
                ConversationName = conversationName,
                Message = message
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.Chat,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(chatClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == ServerMessageType.ChatReceived),
                It.Is<object>(o => (((ChatReceivedServerMessage)o).ConversationName == conversationName) &&
                    (((ChatReceivedServerMessage)o).FromUserName == this.requestUser) &&
                    (((ChatReceivedServerMessage)o).Message == message)),
                It.Is<string[]>(x => x.Length == 1 && x[0] == "user2")));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleTypingChat_Then_Success()
        {
            var conversationName = "Conversation 1";
            var typingMessage = "user2 is typing...";
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { this.requestUser, "user2" }
            };

            this.conversationServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == conversationName)))
               .Returns(conversation)
               .Verifiable();

            var typingChatClientMessage = new TypingChatClientMessage
            {
                UserName = this.requestUser,
                ConversationName = conversationName,
                Message = typingMessage
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.TypingChat,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(typingChatClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == ServerMessageType.TypingChatReceived),
                It.Is<object>(o => (((TypingChatReceivedServerMessage)o).ConversationName == conversationName) &&
                    (((TypingChatReceivedServerMessage)o).FromUserName == this.requestUser) &&
                    (((TypingChatReceivedServerMessage)o).Message == typingMessage)),
                It.Is<string[]>(x => x.Length == 1 && x[0] == "user2")));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleLeaveConversation_Then_Success()
        {
            var conversationName = "Conversation 1";
            var conversation = new Conversation
            {
                Name = conversationName,
                Participants = new List<string> { this.requestUser, "user2" }
            };

            this.conversationServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == conversationName)))
               .Returns(conversation)
               .Verifiable();
            this.conversationServiceMock
               .Setup(s => s.DeleteConversation(It.Is<string>(x => x == conversationName)))
               .Verifiable();

            var leaveConversationClientMessage = new LeaveConversationClientMessage
            {
                UserName = this.requestUser,
                ConversationName = conversationName
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.LeaveConversation,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(leaveConversationClientMessage)
            };
            var canHandle = this.conversationsPluginComponent.CanHandleClientMessage(clientContract);

            this.conversationsPluginComponent.HandleClientMessage(clientContract);

            this.conversationServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == ServerMessageType.ConversationLeft),
                It.Is<object>(o => (((ConversationLeftServerMessage)o).ConversationName == conversationName) &&
                    (((ConversationLeftServerMessage)o).UserName == this.requestUser)),
                It.Is<string[]>(x => x.Length == 1 && x[0] == "user2")));

            Assert.IsTrue(canHandle);
        }
    }
}
