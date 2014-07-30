using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace ThinkUp.Sdk.Tests.TestModels
{
    public class TestPluginComponent : PluginComponentBase
    {
        public TestPluginComponent(INotificationService notificationService, ISerializer serializer)
            : base(notificationService, serializer)
        {

        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return true;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return true;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            return;
        }
    }
}
