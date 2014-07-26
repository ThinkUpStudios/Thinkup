using ThinkUp.Sdk.Data;
using ThinkUp.Sdk.Data.Configuration;
using ThinkUp.Sdk.Plugins;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;
using ThinkUp.Sdk.Setup.Dependencies;

namespace ThinkUp.Sdk.Setup
{
    public class ThinkUpConfigurator : IConfigurator
    {
        public void ConfigureDependencies(IDependencyContainerBuilder dependencyModuleBuilder)
        {
            var configuration = DataSection.Instance() as DataSection;

            dependencyModuleBuilder.SetDependency<IDataSection, DataSection>(configuration);
            dependencyModuleBuilder.SetOpenGenericDependency(typeof(IRepository<>), typeof(Repository<>));
            dependencyModuleBuilder.SetDependency<ISerializer, JsonSerializer>();
            dependencyModuleBuilder.SetDependency<INotificationService, NotificationService>();
            dependencyModuleBuilder.SetDependency<IUserService, UserService>();
            dependencyModuleBuilder.SetDependency<IConversationService, ConversationService>();
            dependencyModuleBuilder.SetDependency<IPluginComponent, UsersPluginComponent>();
            dependencyModuleBuilder.SetDependency<IPluginComponent, ConversationsPluginComponent>();
            dependencyModuleBuilder.SetDependency<IPluginBuilder, PluginBuilder>();
        }
    }
}
