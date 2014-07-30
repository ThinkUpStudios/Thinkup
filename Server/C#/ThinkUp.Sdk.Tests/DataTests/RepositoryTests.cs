using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThinkUp.Sdk.Data;
using ThinkUp.Sdk.Data.Configuration;
using ThinkUp.Sdk.Data.Entities;

namespace ThinkUp.Sdk.Tests.DataTests
{
    [TestClass]
    public abstract class RepositoryTests<T> where T : DataEntity
    {
        private readonly bool cleanDbWhenFinishes;

        protected IRepository<T> testRepository;

        protected RepositoryTests(bool cleanDbWhenFinishes = true)
        {
            this.cleanDbWhenFinishes = cleanDbWhenFinishes;
        }

        [TestInitialize]
        public void Initialize()
        {
            var configuration = DataSection.Instance();

            this.testRepository = new Repository<T>(configuration);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.cleanDbWhenFinishes)
            {
                this.testRepository.DeleteAll();
            }
        }

        protected static string GetUniqueName(string name = null)
        {
            var uniqueName = Guid.NewGuid().ToString();

            return string.IsNullOrEmpty(name) ? uniqueName : string.Concat(name, "-", uniqueName);
        }
    }
}
