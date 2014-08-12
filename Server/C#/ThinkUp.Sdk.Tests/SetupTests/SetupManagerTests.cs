using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ThinkUp.Sdk.Plugins;
using ThinkUp.Sdk.Setup;
using ThinkUp.Sdk.Tests.TestModels;

namespace ThinkUp.Sdk.Tests.SetupTests
{
    [TestClass]
    public class SetupManagerTests
    {
        [TestMethod]
        public void When_Setup_Then_Success()
        {
            var testConfigutator = new TestConfigurator();
            var setupManager = new SetupManager();

            setupManager.AddConfigurator(testConfigutator);

            var plugin = setupManager.GetPlugin() as IPluginSetup;

            Assert.IsNotNull(plugin);
            Assert.AreEqual(4, plugin.Components.Count());
        }
    }
}
