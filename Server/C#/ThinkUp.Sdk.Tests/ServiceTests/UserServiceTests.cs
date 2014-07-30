using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Services;
using ThinkUp.Sdk.Tests.TestModels;

namespace ThinkUp.Sdk.Tests.ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private IUserService userService;

        [TestInitialize]
        public void Initialize()
        {
            var userRepository = new TestRepository<User>();

            this.userService = new UserService(userRepository);
        }

        [TestMethod]
        public void UT_When_ConnectUser_Then_Success()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");

            var users = this.userService.GetAllConnected();

            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count());
        }

        [TestMethod]
        public void UT_When_DisconnectUser_Then_Success()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");

            this.userService.Disconnect("user1");

            var users = this.userService.GetAllConnected();

            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count());
        }

        [TestMethod]
        public void UT_When_GetRandomWithoutUsers_Then_ReturnNull()
        {
            this.userService.Connect("user1", "User 1");

            var existingUser = this.userService.GetRandom(userNameToExclude: "user1");

            Assert.IsNull(existingUser);
        }

        [TestMethod]
        public void UT_When_GetRandomWithUsers_Then_Sucess()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");

            var existingUser = this.userService.GetRandom(userNameToExclude: "user1");

            Assert.IsNotNull(existingUser);
            Assert.AreEqual("user2", existingUser.Name);
            Assert.AreEqual("User 2", existingUser.DisplayName);
        }

        [TestMethod]
        public void UT_When_GetByName_Then_Sucess()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");
            this.userService.Connect("User3", "User 3");

            var user2 = this.userService.GetByName("user2");

            Assert.IsNotNull(user2);
            Assert.AreEqual("user2", user2.Name);
            Assert.AreEqual("User 2", user2.DisplayName);
            Assert.IsTrue(user2.IsConnected);
        }

        [TestMethod]
        public void UT_When_GetAllConnectedWithoutExcluding_Then_Sucess()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");
            this.userService.Connect("user3", "User 3");
            this.userService.Connect("user4", "User 4");

            this.userService.Disconnect("user1");
            this.userService.Disconnect("user3");

            var users = this.userService.GetAllConnected();

            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count());
        }

        [TestMethod]
        public void UT_When_GetAllConnectedExcluding_Then_Sucess()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");
            this.userService.Connect("user3", "User 3");
            this.userService.Connect("user4", "User 4");

            this.userService.Disconnect("user1");
            this.userService.Disconnect("user3");

            var users = this.userService.GetAllConnected(userNameToExclude: "user4");

            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count());
        }

        [TestMethod]
        public void UT_When_GetAllWithoutExcluding_Then_Sucess()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");
            this.userService.Connect("user3", "User 3");
            this.userService.Connect("user4", "User 4");

            this.userService.Disconnect("user1");
            this.userService.Disconnect("user3");

            var users = this.userService.GetAll();

            Assert.IsNotNull(users);
            Assert.AreEqual(4, users.Count());
        }

        [TestMethod]
        public void UT_When_GetAllExcluding_Then_Sucess()
        {
            this.userService.Connect("user1", "User 1");
            this.userService.Connect("user2", "User 2");
            this.userService.Connect("user3", "User 3");
            this.userService.Connect("user4", "User 4");

            this.userService.Disconnect("user1");
            this.userService.Disconnect("user3");

            var users = this.userService.GetAll(userNameToExclude: "user1");

            Assert.IsNotNull(users);
            Assert.AreEqual(3, users.Count());
        }
    }
}
