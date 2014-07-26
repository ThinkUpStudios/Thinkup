using ThinkUp.Sdk.Plugins;

namespace ThinkUp.Sdk.Setup
{
    public interface ISetupManager
    {
        void AddConfigurator(IConfigurator configurator);

        IPlugin GetPlugin();
    }
}
