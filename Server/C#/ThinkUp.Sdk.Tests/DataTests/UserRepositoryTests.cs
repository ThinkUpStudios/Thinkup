using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ThinkUp.Sdk.Data.Entities;

namespace ThinkUp.Sdk.Tests.DataTests
{
    [TestClass]
    public class UserRepositoryTests : RepositoryTests<User>
    {
        [TestMethod]
        public void UT_When_CreateUser_Then_Success()
        {
            var userName = GetUniqueName("user");
            var user = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = userName
            };

            this.testRepository.Create(user);

            var createdUser = this.testRepository.Get(e => e.Name == userName);

            Assert.IsNotNull(createdUser);
            Assert.AreEqual(user.Id, createdUser.Id);
            Assert.AreEqual(user.DisplayName, createdUser.DisplayName);
            Assert.AreEqual(userName, createdUser.Name);
        }

        [TestMethod]
        public void UT_When_UpdateUser_Then_Success()
        {
            var userName = GetUniqueName("user");
            var user = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = userName
            };

            this.testRepository.Create(user);

            var createdUser = this.testRepository.Get(e => e.Name == userName);

            createdUser.DisplayName = GetUniqueName("Updated User");

            this.testRepository.Update(createdUser);

            var updatedUser = this.testRepository.Get(e => e.Name == userName);

            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(user.Id, updatedUser.Id);
            Assert.AreEqual(createdUser.DisplayName, updatedUser.DisplayName);
        }

        [TestMethod]
        public void UT_When_DeleteUser_Then_Success()
        {
            var userName = GetUniqueName("user");
            var user = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = userName
            };

            this.testRepository.Create(user);

            var createdUser = this.testRepository.Get(e => e.Name == userName);

            this.testRepository.Delete(createdUser);

            var deletedUser = this.testRepository.Get(e => e.Name == userName);

            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public void UT_When_DeleteAllUsers_Then_Success()
        {
            var user1Name = GetUniqueName("user");
            var user1 = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = user1Name
            };
            var user2Name = GetUniqueName("user");
            var user2 = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = user2Name
            };
            var user3Name = GetUniqueName("user");
            var user3 = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = user3Name
            };

            this.testRepository.Create(user1);
            this.testRepository.Create(user2);
            this.testRepository.Create(user3);

            var currentUsersCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var existingEntities = this.testRepository.GetAll();

            Assert.AreEqual(3, currentUsersCount);
            Assert.AreEqual(0, existingEntities.Count());
        }

        [TestMethod]
        public void UT_When_GetUsersWithPredicate_Then_Success()
        {
            var user1Name = GetUniqueName("user");
            var user1 = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = user1Name
            };
            var user2Name = GetUniqueName("user");
            var user2 = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = user2Name
            };
            var user3Name = GetUniqueName("user");
            var user3 = new User
            {
                DisplayName = GetUniqueName("User"),
                Name = user3Name
            };
            var user4Name = GetUniqueName("test user");
            var user4 = new User
            {
                DisplayName = GetUniqueName("Test User"),
                Name = user4Name
            };

            this.testRepository.Create(user1);
            this.testRepository.Create(user2);
            this.testRepository.Create(user3);
            this.testRepository.Create(user4);

            var testUsers = this.testRepository.GetAll(e => e.DisplayName.Contains("Test"));

            Assert.AreEqual(1, testUsers.Count());
        }
    }
}
