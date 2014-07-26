using System;

namespace ThinkUp.Sdk.Setup.Dependencies
{
    public interface IDependencyContainerBuilder
    {
        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency<T>();

        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency(Type type);

        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency<T, U>() where U : T;

        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency(Type interfaceType, Type instanceType);

        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency<T>(Type instanceType);

        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency<T>(T instance) where T : class;

        ///<exception cref="SetupException">SetupException</exception>
        void SetDependency<T, U>(U instance) where U : class, T;

        ///<exception cref="SetupException">SetupException</exception>
        void SetOpenGenericDependency(Type openGenericInterfaceType, Type openGenericType);

        ///<exception cref="SetupException">SetupException</exception>
        IDependencyContainer Build();
    }
}
