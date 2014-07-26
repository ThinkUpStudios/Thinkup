using Autofac;
using System;

namespace ThinkUp.Sdk.Setup.Dependencies
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IContainer container;

        public DependencyContainer(IContainer container)
        {
            this.container = container;
        }

        ///<exception cref="SetupException">SetupException</exception>
        public object Get(Type objectType)
        {
            try
            {
                return this.container.Resolve(objectType);
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An error occurred when trying to resolve dependency for {0}. Details: {1}", objectType.Name, ex.Message);

                throw new SetupException(errorMessage, ex);
            }
        }

        ///<exception cref="SetupException">SetupException</exception>
        public T Get<T>()
        {
            try
            {
                return this.container.Resolve<T>();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An error occurred when trying to resolve dependency for {0}. Details: {1}", typeof(T).Name, ex.Message);

                throw new SetupException(errorMessage, ex);
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.container != null)
                {
                    this.container.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
