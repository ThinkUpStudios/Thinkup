using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkUp.Sdk.Data;
using ThinkUp.Sdk.Data.Configuration;
using ThinkUp.Sdk.Setup.Dependencies;
using ThinkUp.Sdk.Tests.TestModels;

namespace ThinkUp.Sdk.Tests.SetupTests
{
    [TestClass]
    public class DependencyContainerTests
    {
        [TestMethod]
        public void UT_When_BuildDependencies_Then_Success()
        {
            var configuration = DataSection.Instance();
            var dependencyContainerBuilder = new DependencyContainerBuilder();

            dependencyContainerBuilder.SetDependency<ITestServiceBar, TestServiceBar>();
            dependencyContainerBuilder.SetDependency<ITestServiceFoo, TestServiceFoo>();
            dependencyContainerBuilder.SetDependency<IDataSection>(configuration);
            dependencyContainerBuilder.SetOpenGenericDependency(typeof(IRepository<>), typeof(Repository<>));

            var dependencyContainer = dependencyContainerBuilder.Build();

            var testServiceFoo = dependencyContainer.Get<ITestServiceFoo>();
            var typedRepository = dependencyContainer.Get<IRepository<TestEntity>>();

            Assert.IsNotNull(testServiceFoo);
            Assert.IsNotNull(testServiceFoo.TestServiceBar);
            Assert.IsNotNull(typedRepository);
        }
    }
}
