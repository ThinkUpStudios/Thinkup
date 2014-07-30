using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ThinkUp.Sdk.Tests.TestModels;

namespace ThinkUp.Sdk.Tests.DataTests
{
    [TestClass]
    public class TestRepositoryTests : RepositoryTests<TestEntity>
    {
        [TestMethod]
        public void UT_When_CreateTestEntity_Then_Success()
        {
            var testEntityName = GetUniqueName("test");
            var testEntity = new TestEntity
            {
                Name = testEntityName,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNotNull(createdTestEntity);
            Assert.AreEqual(testEntity.Id, createdTestEntity.Id);
            Assert.AreEqual(testEntity.DisplayName, createdTestEntity.DisplayName);
            Assert.AreEqual(true, createdTestEntity.IsValid);
        }

        [TestMethod]
        public void UT_When_UpdateTestEntity_Then_Success()
        {
            var testEntityName = GetUniqueName("test");
            var testEntity = new TestEntity
            {
                Name = testEntityName,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            createdTestEntity.IsValid = false;
            createdTestEntity.DisplayName = GetUniqueName("Updated Test");

            this.testRepository.Update(createdTestEntity);

            var updatedTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNotNull(updatedTestEntity);
            Assert.AreEqual(testEntity.Id, updatedTestEntity.Id);
            Assert.AreEqual(createdTestEntity.DisplayName, updatedTestEntity.DisplayName);
            Assert.AreEqual(false, updatedTestEntity.IsValid);
        }

        [TestMethod]
        public void UT_When_DeleteTestEntity_Then_Success()
        {
            var testEntityName = GetUniqueName("test");
            var testEntity = new TestEntity
            {
                Name = testEntityName,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            this.testRepository.Delete(createdTestEntity);

            var deletedTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNull(deletedTestEntity);
        }

        [TestMethod]
        public void UT_When_DeleteAllTestEntities_Then_Success()
        {
            var testEntityName1 = GetUniqueName("test");
            var testEntity1 = new TestEntity
            {
                Name = testEntityName1,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };
            var testEntityName2 = GetUniqueName("test");
            var testEntity2 = new TestEntity
            {
                Name = testEntityName2,
                DisplayName = GetUniqueName("Test"),
                IsValid = false
            };
            var testEntityName3 = GetUniqueName("test");
            var testEntity3 = new TestEntity
            {
                Name = testEntityName3,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };

            this.testRepository.Create(testEntity1);
            this.testRepository.Create(testEntity2);
            this.testRepository.Create(testEntity3);

            var currentEntitiesCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var existingEntities = this.testRepository.GetAll();

            Assert.AreEqual(3, currentEntitiesCount);
            Assert.AreEqual(0, existingEntities.Count());
        }

        [TestMethod]
        public void UT_When_GetTestEntitiesWithPredicate_Then_Success()
        {
            var testEntityName1 = GetUniqueName("test");
            var testEntity1 = new TestEntity
            {
                Name = testEntityName1,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };
            var testEntityName2 = GetUniqueName("test");
            var testEntity2 = new TestEntity
            {
                Name = testEntityName2,
                DisplayName = GetUniqueName("Test"),
                IsValid = false
            };
            var testEntityName3 = GetUniqueName("test");
            var testEntity3 = new TestEntity
            {
                Name = testEntityName3,
                DisplayName = GetUniqueName("Test"),
                IsValid = true
            };
            var testEntityName4 = GetUniqueName("entity");
            var testEntity4 = new TestEntity
            {
                Name = testEntityName4,
                DisplayName = GetUniqueName("Entity"),
                IsValid = true
            };

            this.testRepository.Create(testEntity1);
            this.testRepository.Create(testEntity2);
            this.testRepository.Create(testEntity3);
            this.testRepository.Create(testEntity4);

            var validEntities = this.testRepository.GetAll(e => e.IsValid);
            var testEntities = this.testRepository.GetAll(e => e.Name.StartsWith("test"));
            var entitiesWith4 = this.testRepository.GetAll(e => e.DisplayName.Contains("Entity"));

            Assert.AreEqual(3, validEntities.Count());
            Assert.AreEqual(3, testEntities.Count());
            Assert.AreEqual(1, entitiesWith4.Count());
        }
    }
}
