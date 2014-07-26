using System.Collections.Generic;
using ThinkUp.Sdk.Plugins.PluginComponents;

namespace ThinkUp.Sdk.Plugins
{
    public interface IPluginBuilder
    {
        void RegisterPluginComponents(IEnumerable<IPluginComponent> pluginComponents);

        void RegisterPluginComponent(IPluginComponent pluginComponent);

        IPlugin Build();
    }
}
