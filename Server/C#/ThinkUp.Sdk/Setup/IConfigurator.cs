using ThinkUp.Sdk.Setup.Dependencies;

namespace ThinkUp.Sdk.Setup
{
    public interface IConfigurator
    {
        void ConfigureDependencies(IDependencyContainerBuilder dependencyContainerBuilder);
    }
}
