using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System.Collections.Generic;
using System.Reflection;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Setup;

namespace ThinkUp.SignalR
{
    public abstract class ThinkUpStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var containerBuilder = new ContainerBuilder();

                containerBuilder.RegisterType<JsonSerializer>().As<ISerializer>().SingleInstance();
                containerBuilder.RegisterType<UserConnectionMapper>().As<IUserConnectionMapper>().SingleInstance();

                var setupManager = new SetupManager();
                var configurators = this.GetConfigurators();

                foreach (var configurator in configurators)
                {
                    setupManager.AddConfigurator(configurator);
                }

                containerBuilder.RegisterInstance(setupManager).As<ISetupManager>().SingleInstance();
                containerBuilder.RegisterType<ClientManager>().As<IClientManager>().SingleInstance();
                containerBuilder.RegisterHubs(Assembly.GetExecutingAssembly());

                var container = containerBuilder.Build();
                var resolver = new AutofacDependencyResolver(container);

                GlobalHost.DependencyResolver = resolver;

                var configuration = new HubConfiguration();

                configuration.EnableJSONP = true;

                map.RunSignalR(configuration);
            });
        }

        public abstract IEnumerable<IConfigurator> GetConfigurators();
    }
}