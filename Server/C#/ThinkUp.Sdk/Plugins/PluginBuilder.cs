using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Plugins.PluginComponents;

namespace ThinkUp.Sdk.Plugins
{
    public class PluginBuilder : IPluginBuilder
    {
        private readonly IList<IPluginComponent> pluginComponents;
        private readonly ISerializer serializer;

        public PluginBuilder(ISerializer serializer)
        {
            this.pluginComponents = new List<IPluginComponent>();
            this.serializer = serializer;
        }

        public void RegisterPluginComponents(IEnumerable<IPluginComponent> pluginComponents)
        {
            foreach (var pluginComponent in pluginComponents)
            {
                this.RegisterPluginComponent(pluginComponent);
            }
        }

        public void RegisterPluginComponent(IPluginComponent pluginComponent)
        {
            if (!this.pluginComponents.Any(c => c.Name == pluginComponent.Name))
            {
                this.pluginComponents.Add(pluginComponent);
            }
        }

        public IPlugin Build()
        {
            var plugin = new Plugin(this.serializer);

            foreach (var pluginComponent in this.pluginComponents)
            {
                plugin.RegisterComponent(pluginComponent);
            }

            return plugin;
        }
    }
}
