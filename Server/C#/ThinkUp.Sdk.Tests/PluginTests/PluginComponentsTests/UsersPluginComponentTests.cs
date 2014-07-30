using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class UsersPluginComponentTests
    {
        private readonly string requestUser = "user1";

        private ISerializer serializer;
        private IUser user1;
        private IUser user2;
        private IList<IUser> users;
        private Mock<IUserService> userServiceMock;
        private Mock<INotificationService> notificationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.user1 = new User
            {
                DisplayName = "User 1",
                Name = "user1"
            };
            this.user2 = new User
            {
                DisplayName = "User 2",
                Name = "user2"
            };

            var user3 = new User
            {
                DisplayName = "User 3",
                Name = "user3"
            };
            var user4 = new User
            {
                DisplayName = "User 4",
                Name = "user4"
            };

            this.users = new List<IUser>();
            this.users.Add(user2);
            this.users.Add(user3);
            this.users.Add(user4);

            this.userServiceMock = new Mock<IUserService>();

            this.notificationServiceMock = new Mock<INotificationService>();
        }

        [TestMethod]
        public void UT_When_HandleGetConnectedUsers_Then_Success()
        {
            this.userServiceMock
               .Setup(s => s.GetAllConnected(It.Is<string>(x => x == this.requestUser)))
               .Returns(this.users)
               .Verifiable();

            var getConnectedUsersClientMessage = new GetConnectedUsersClientMessage
            {
                UserName = this.requestUser,
                PageSize = 10
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.GetConnectedUsers,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(getConnectedUsersClientMessage)
            };

            var usersPluginComponent = this.GetUsersPluginComponent();
            var canHandle = usersPluginComponent.CanHandleClientMessage(clientContract);

            usersPluginComponent.HandleClientMessage(clientContract);

            this.userServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == ServerMessageType.ConnectedUsersList),
                It.Is<object>(o => (((ConnectedUsersListServerMessage)o).ConectedUsersCount == 3)
                    && ((ConnectedUsersListServerMessage)o).UserName == this.requestUser),
                It.Is<string>(x => x == this.requestUser)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleConnectUser_Then_Success()
        {
            this.userServiceMock
                .Setup(s => s.GetAllConnected(It.Is<string>(x => x == this.requestUser)))
                .Returns(new List<IUser> { this.user2 })
                .Verifiable();

            var connectUserClientMessage = new ConnectUserClientMessage
            {
                UserName = this.requestUser
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.ConnectUser,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(connectUserClientMessage)
            };

            this.userServiceMock
                .Setup(s => s.Connect(It.Is<string>(x => x == this.requestUser), It.IsAny<string>()))
                .Verifiable();

            var usersPluginComponent = this.GetUsersPluginComponent();
            var canHandle = usersPluginComponent.CanHandleClientMessage(clientContract);

            usersPluginComponent.HandleClientMessage(clientContract);

            this.userServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == ServerMessageType.UserConnected),
                    It.Is<object>(o => ((UserConnectedServerMessage)o).UserName == this.requestUser),
                    It.Is<string>(x => x == this.user2.Name)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleDisconnectUser_Then_Success()
        {
            this.userServiceMock
                .Setup(s => s.GetAllConnected(It.Is<string>(x => x == this.requestUser)))
                .Returns(new List<IUser> { this.user2 })
                .Verifiable();

            var disconnectUserClientMessage = new DisconnectUserClientMessage
            {
                UserName = this.requestUser,
            };
            var clientContract = new ClientContract
            {
                Type = ClientMessageType.DisconnectUser,
                Sender = this.requestUser,
                SerializedClientMessage = this.serializer.Serialize(disconnectUserClientMessage)
            };

            var usersPluginComponent = this.GetUsersPluginComponent();
            var canHandle = usersPluginComponent.CanHandleClientMessage(clientContract);

            usersPluginComponent.HandleClientMessage(clientContract);

            this.userServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == ServerMessageType.UserDisconnected),
                    It.Is<object>(o => ((UserDisconnectedServerMessage)o).UserName == this.requestUser),
                    It.Is<string>(x => x == this.user2.Name)));

            Assert.IsTrue(canHandle);
        }

        private IPluginComponent GetUsersPluginComponent()
        {
            return new UsersPluginComponent(this.userServiceMock.Object, this.notificationServiceMock.Object, this.serializer);
        }
    }
}
