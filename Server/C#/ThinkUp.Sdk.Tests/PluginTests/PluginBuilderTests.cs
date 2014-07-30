using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Plugins;
using ThinkUp.Sdk.Plugins.PluginComponents;

namespace ThinkUp.Sdk.Tests.PluginTests
{
    [TestClass]
    public class PluginBuilderTests
    {
        private readonly string userName = "user1";

        private ISerializer serializer;
        private readonly int testClientContractType = 999;
        private IClientMessage testClientMessage;
        private ClientContract testClientContract;
        private Mock<IPluginComponent> testPluginComponentMock;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();
            this.testClientMessage = Mock.Of<IClientMessage>();

            this.testClientContract = new ClientContract
            {
                Type = this.testClientContractType,
                Sender = this.userName,
                SerializedClientMessage = this.serializer.Serialize(this.testClientMessage)
            };

            this.testPluginComponentMock = new Mock<IPluginComponent>();

            this.testPluginComponentMock
                .Setup(x => x.CanHandleClientMessage(It.Is<ClientContract>(c => c.Type == this.testClientContract.Type)))
                .Returns(true)
                .Verifiable();
            this.testPluginComponentMock
                .Setup(x => x.HandleClientMessage(It.Is<ClientContract>(c => c.Type == this.testClientContract.Type)))
                .Verifiable();
        }

        [TestMethod]
        public void When_BuildPlugin_Then_Success()
        {
            var pluginBuilder = new PluginBuilder(this.serializer);

            pluginBuilder.RegisterPluginComponent(this.testPluginComponentMock.Object);

            var plugin = pluginBuilder.Build();
            var serializedClientMessage = this.serializer.Serialize(this.testClientContract);
            var canHandleTestMessage = plugin.CanHandleClientMessage(serializedClientMessage);

            plugin.HandleClientMessage(serializedClientMessage);

            Assert.IsTrue(canHandleTestMessage);

            this.testPluginComponentMock.VerifyAll();
        }
    }
}
