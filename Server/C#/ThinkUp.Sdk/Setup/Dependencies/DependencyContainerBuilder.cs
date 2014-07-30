using Autofac;
using System;

namespace ThinkUp.Sdk.Setup.Dependencies
{
    public class DependencyContainerBuilder : IDependencyContainerBuilder
    {
        private readonly ContainerBuilder containerBuilder;

        public DependencyContainerBuilder()
        {
            this.containerBuilder = new ContainerBuilder();
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency<T>()
        {
            this.SetDependency(() => 
            {
                this.containerBuilder.RegisterType<T>();
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency(Type type)
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterType(type);
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency<T, U>() where U : T
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterType<U>().As<T>();
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency(Type interfaceType, Type instanceType)
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterType(instanceType).As(interfaceType);
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency<T>(Type instanceType)
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterType(instanceType).As<T>();
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency<T>(T instance) where T : class
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterInstance(instance).As<T>();
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetDependency<T, U>(U instance) where U : class, T
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterInstance(instance).As<T>();
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public void SetOpenGenericDependency(Type openGenericInterfaceType, Type openGenericType)
        {
            this.SetDependency(() =>
            {
                this.containerBuilder.RegisterGeneric(openGenericType).As(openGenericInterfaceType);
            });
        }

        ///<exception cref="SetupException">SetupException</exception>
        public IDependencyContainer Build()
        {
            var container = default(IContainer);

            try
            {
                container = this.containerBuilder.Build();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An unexpected error occurred when building dependencies. Details: {0}", ex.Message);

                throw new SetupException(errorMessage, ex);
            }

            var dependencyModule = new DependencyContainer(container);

            return dependencyModule;
        }

        private void SetDependency(Action setDependencyAction)
        {
            try
            {
                setDependencyAction.Invoke();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An unexpected error occurred when setting up dependencies. Details: {0}", ex.Message);

                throw new SetupException(errorMessage, ex);
            }
        }
    }
}
