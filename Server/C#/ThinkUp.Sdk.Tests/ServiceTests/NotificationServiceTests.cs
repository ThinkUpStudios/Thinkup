using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkUp.Sdk.Services;
using ThinkUp.Sdk.Tests.TestModels;

namespace ThinkUp.Sdk.Tests.ServiceTests
{
    [TestClass]
    public class NotificationServiceTests
    {
        private ISerializer serializer;
        private INotificationService notificationService;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.notificationService = new NotificationService(this.serializer);
        }

        [TestMethod]
        public void UT_When_Send_Then_Success()
        {
            var userName = "player1";
            var serverMessageType = 204;
            var serverMessage = new TestServerMessage
            {
                Name = "Test 1"
            };

            var notifiedUserName = default(string);
            var notifiedType = default(int);
            var notifiedObject = default(TestServerMessage);

            this.notificationService.Notification += (sender, e) =>
            {
                notifiedUserName = e.Receiver;
                notifiedType = e.NotificationType;
                notifiedObject = (TestServerMessage)e.Notification;
            };

            this.notificationService.Send(serverMessageType, serverMessage, userName);

            Assert.AreEqual(userName, notifiedUserName);
            Assert.AreEqual(serverMessageType, notifiedType);
            Assert.AreEqual(serverMessage.Name, notifiedObject.Name);
            Assert.AreEqual(serverMessage.Message, notifiedObject.Message);
        }
    }
}
