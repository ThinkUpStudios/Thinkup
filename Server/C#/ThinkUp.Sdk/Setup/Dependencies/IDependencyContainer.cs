using System;

namespace ThinkUp.Sdk.Setup.Dependencies
{
    public interface IDependencyContainer : IDisposable
    {
        ///<exception cref="SetupException">SetupException</exception>
        object Get(Type objectType);

        ///<exception cref="SetupException">SetupException</exception>
        T Get<T>();
    }
}
