using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Setup;
using ThinkUp.Sdk.Setup.Dependencies;

namespace ThinkUp.Sdk.Tests.TestModels
{
    public class TestConfigurator : IConfigurator
    {
        public void ConfigureDependencies(IDependencyContainerBuilder dependencyContainerBuilder)
        {
            dependencyContainerBuilder.SetDependency<IPluginComponent, TestPluginComponent>();
        }
    }
}
