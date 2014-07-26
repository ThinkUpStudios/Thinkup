using System.Collections.Generic;
using ThinkUp.Sdk.Plugins;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Setup.Dependencies;

namespace ThinkUp.Sdk.Setup
{
    public class SetupManager : ISetupManager
    {
        private readonly IList<IConfigurator> configurators;

        public SetupManager()
        {
            this.configurators = new List<IConfigurator>();
            this.AddConfigurator(new ThinkUpConfigurator());
        }

        public void AddConfigurator(IConfigurator setup)
        {
            this.configurators.Add(setup);
        }

        public IPlugin GetPlugin()
        {
            var dependencyContainerBuilder = new DependencyContainerBuilder();

            foreach (var configurator in this.configurators)
            {
                configurator.ConfigureDependencies(dependencyContainerBuilder);
            }

            var dependencyContainer = dependencyContainerBuilder.Build();
            var pluginComponents = dependencyContainer.Get<IEnumerable<IPluginComponent>>();
            var pluginBuilder = dependencyContainer.Get<IPluginBuilder>();

            pluginBuilder.RegisterPluginComponents(pluginComponents);

            return pluginBuilder.Build();
        }
    }
}
