using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Services;
using ThinkUp.Sdk.Tests.TestModels;

namespace ThinkUp.Sdk.Tests.ServiceTests
{
    [TestClass]
    public class ConversationServiceTests
    {
        private IConversationService conversationService;

        [TestInitialize]
        public void Initialize()
        {
            var conversationRepository = new TestRepository<Conversation>();

            this.conversationService = new ConversationService(conversationRepository);
        }

        [TestMethod]
        public void When_CreateConversationWithName_Then_Success()
        {
            var conversationName = GetUniqueName("Conversation");
            var userNames = new List<string> { "user1", "user2", "user3" };

            this.conversationService.CreateConversation(conversationName, userNames.ToArray());

            var existingConversation = this.conversationService.GetByName(conversationName);

            Assert.IsNotNull(existingConversation);
            Assert.AreEqual(userNames.Count, existingConversation.GetParticipants().Count());
        }

        [TestMethod]
        public void When_CreateConversationWithoutName_Then_Success()
        {
            var userNames = new string[] { "user1", "user2", "user3" };

            this.conversationService.CreateConversation(userNames);

            var existingConversations = this.conversationService.GetAllByParticipant("user1");

            Assert.IsNotNull(existingConversations);
            Assert.AreEqual(1, existingConversations.Count());
            Assert.AreEqual(string.Join(" & ", userNames.Sort()), existingConversations.First().Name);
            Assert.AreEqual(userNames.Length, existingConversations.First().GetParticipants().Count());
        }

        [TestMethod]
        public void When_CreateConversationWithoutEnoughParticipants_Then_Fail()
        {
            var userNames = new string[] { "user1" };

            try
            {
                this.conversationService.CreateConversation(userNames);
                Assert.Fail("Create Conversation with less than two users should have failed");
            }
            catch (ServiceException serviceEx)
            {
                var expectedErrorMessage = string.Format("A conversation must have at least two participants. {0} participant is not allowed", userNames.Length);

                Assert.AreEqual(expectedErrorMessage, serviceEx.Message);
            }
        }

        [TestMethod]
        public void When_AddMessageToConversation_Then_Success()
        {
            var userNames = new string[] { "user1", "user2", "user3" };

            this.conversationService.CreateConversation(userNames);

            var existingConversations = this.conversationService.GetAllByParticipant("user1");
            var existingConversation = existingConversations.First();

            this.conversationService.AddMessage(existingConversation.Name, "user1", "Message 1");

            var updatedConversation = this.conversationService.GetByName(existingConversation.Name);

            Assert.IsNotNull(updatedConversation);
            Assert.AreEqual(1, updatedConversation.GetMessages().Count());
            Assert.AreEqual("user1", updatedConversation.GetMessages().First().Sender);
            Assert.AreEqual("Message 1", updatedConversation.GetMessages().First().Content);
        }

        [TestMethod]
        public void When_AddParticipantToConversation_Then_Success()
        {
            var userNames = new string[] { "user1", "user2" };

            this.conversationService.CreateConversation(userNames);

            var existingConversations = this.conversationService.GetAllByParticipant("user1");
            var existingConversation = existingConversations.First();

            this.conversationService.AddParticipant(existingConversation.Name, "user3");

            var updatedConversation = this.conversationService.GetByName(existingConversation.Name);

            Assert.IsNotNull(updatedConversation);
            Assert.AreEqual(3, updatedConversation.GetParticipants().Count());
        }

        [TestMethod]
        public void When_GetAllConversationsByParticipant_Then_Success()
        {
            var userNames1 = new string[] { "user1", "user2" };
            var userNames2 = new string[] { "user1", "user3" };
            var userNames3 = new string[] { "user3", "user4" };

            this.conversationService.CreateConversation(userNames1);
            this.conversationService.CreateConversation(userNames2);
            this.conversationService.CreateConversation(userNames3);

            var conversationsOfUser1 = this.conversationService.GetAllByParticipant("user1");
            var conversationsOfUser2 = this.conversationService.GetAllByParticipant("user2");
            var conversationsOfUser3 = this.conversationService.GetAllByParticipant("user3");
            var conversationsOfUser4 = this.conversationService.GetAllByParticipant("user4");
            var conversationsOfUser5 = this.conversationService.GetAllByParticipant("user5");

            Assert.AreEqual(2, conversationsOfUser1.Count());
            Assert.AreEqual(1, conversationsOfUser2.Count());
            Assert.AreEqual(2, conversationsOfUser3.Count());
            Assert.AreEqual(1, conversationsOfUser4.Count());
            Assert.AreEqual(0, conversationsOfUser5.Count());
        }

        [TestMethod]
        public void When_GetConversationsByName_Then_Success()
        {
            var userNames1 = new string[] { "user1", "user2" };
            var userNames2 = new string[] { "user1", "user3" };
            var userNames3 = new string[] { "user3", "user4" };

            this.conversationService.CreateConversation(userNames1);
            this.conversationService.CreateConversation(userNames2);
            this.conversationService.CreateConversation(userNames3);

            var conversationName1 = "user1 & user3";
            var conversationName2 = "user4 & user5";

            Assert.IsNotNull(this.conversationService.GetByName(conversationName1));
            Assert.IsNull(this.conversationService.GetByName(conversationName2));
        }

        [TestMethod]
        public void When_DeleteConversation_Then_Success()
        {
            var userNames1 = new string[] { "user1", "user2" };
            var userNames2 = new string[] { "user1", "user3" };
            var userNames3 = new string[] { "user3", "user4" };

            this.conversationService.CreateConversation(userNames1);
            this.conversationService.CreateConversation(userNames2);
            this.conversationService.CreateConversation(userNames3);

            var user3ConversationsBeforeDeleteCount = this.conversationService.GetAllByParticipant("user3").Count();
            var conversationNameToDelete = "user1 & user3";

            this.conversationService.DeleteConversation(conversationNameToDelete);

            var user3ConversationsAfterDeleteCount = this.conversationService.GetAllByParticipant("user3").Count();

            Assert.AreEqual(2, user3ConversationsBeforeDeleteCount);
            Assert.AreEqual(1, user3ConversationsAfterDeleteCount);
        }

        protected static string GetUniqueName(string name = null)
        {
            var uniqueName = Guid.NewGuid().ToString();

            return string.IsNullOrEmpty(name) ? uniqueName : string.Concat(name, "-", uniqueName);
        }
    }
}
