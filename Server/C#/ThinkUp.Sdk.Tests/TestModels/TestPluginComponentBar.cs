using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Tests.TestModels
{
    public class TestPluginComponentBar : PluginComponentBase
    {
        public TestPluginComponentBar(INotificationService notificationService, ISerializer serializer)
            : base(notificationService, serializer)
        {
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == TestClientMessageType.TestClient;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == TestServerMessageType.TestServer;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            var testClientMessage = this.serializer.Deserialize<TestClientMessage>(clientContract.SerializedClientMessage);
            var testServerMessage = new TestServerMessage
            {
                Name = "Test Bar"
            };

            this.notificationService.Send(TestServerMessageType.TestServer, testServerMessage, testClientMessage.UserName);
        }
    }
}
