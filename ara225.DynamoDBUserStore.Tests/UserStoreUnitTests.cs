using ara225.DynamoDBUserStore;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevProjUnitTests
{
    [TestClass]
    public class UserStoreUnitTests
    {
        private static DynamoDBUserStore _store;
        private static DynamoDBUser _user;
        
        [AssemblyInitialize]
        public static void Initialize(TestContext Context)
        {
            _store = new DynamoDBUserStore(new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient(), "UserStoreTable", "RoleStoreTable"));
            _user = new DynamoDBUser("TestUser");
            _user.Email = "user@example.com";
            _user.NormalizedEmail = "USER@EXAMPLE.COM";
            IdentityResult CreateResult =  _store.CreateAsync(_user, new CancellationToken()).Result;
            Assert.AreEqual(CreateResult, IdentityResult.Success);
        }

        [TestMethod]
        public async Task TestLoginFunctions()
        {
            await _store.AddLoginAsync(_user, new UserLoginInfo("TestProvider", "RandomKey", "TestProviderDN"), new CancellationToken());
            await _store.UpdateAsync(_user, new CancellationToken());

            DynamoDBUser user = await _store.FindByLoginAsync("TestProvider", "RandomKey", new CancellationToken());
            Assert.AreEqual(user.Id, _user.Id);
            Assert.AreEqual(user.LoginProviderKeys.Count, 1);
            Assert.AreEqual(user.LoginProviderKeys[0], "RandomKey");
            IList<UserLoginInfo> LoginInfo = await _store.GetLoginsAsync(_user, new CancellationToken());
            Assert.AreEqual(LoginInfo[0].ProviderKey, "RandomKey");
            await _store.RemoveLoginAsync(_user, "TestProvider", "RandomKey", new CancellationToken());
        }


        [TestMethod]
        public async Task TestFindByEmail()
        {
            DynamoDBUser user = await _store.FindByEmailAsync(_user.NormalizedEmail, new CancellationToken());
            Assert.AreEqual(user.Id, _user.Id);
        }

        [TestMethod]
        public async Task TestFindById()
        {
            DynamoDBUser user = await _store.FindByIdAsync(_user.Id, new CancellationToken());
            Assert.AreEqual(user.Id, _user.Id);
        }

        [TestMethod]
        public async Task TestFindByName()
        {
            DynamoDBUser user = await _store.FindByNameAsync(_user.NormalizedUserName, new CancellationToken());
            Assert.AreEqual(user.Id, _user.Id);
        }

        [TestMethod]
        public async Task TestGetEmail()
        {
            string Email = await _store.GetEmailAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.Email, Email);
        }

        [TestMethod]
        public async Task TestGetEmailConfirmed()
        {
            bool EmailConfirmed = await _store.GetEmailConfirmedAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.EmailConfirmed, EmailConfirmed);
        }

        [TestMethod]
        public async Task TestGetNormalizedEmail()
        {
            string NormalizedEmail = await _store.GetNormalizedEmailAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.NormalizedEmail, NormalizedEmail);
        }

        [TestMethod]
        public async Task TestGetNormalizedUserName()
        {
            string NormalizedUserName = await _store.GetNormalizedUserNameAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.NormalizedUserName, NormalizedUserName);
        }

        [TestMethod]
        public async Task TestGetPasswordHash()
        {
            string PasswordHash = await _store.GetPasswordHashAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.PasswordHash, PasswordHash);
        }

        [TestMethod]
        public async Task TestGetUserId()
        {
            string Id = await _store.GetUserIdAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.Id, Id);
        }

        [TestMethod]
        public async Task TestGetUserName()
        {
            string UserName = await _store.GetUserNameAsync(_user, new CancellationToken());
            Assert.AreEqual(_user.UserName, UserName);
        }

        [TestMethod]
        public async Task TestHasPassword()
        {
            bool HasPassword = await _store.HasPasswordAsync(_user, new CancellationToken());
            Assert.AreEqual(HasPassword, true);
        }

        [TestMethod]
        public async Task TestSetEmail()
        {
            await _store.SetEmailAsync(_user, "user@example.com", new CancellationToken());
        }

        [TestMethod]
        public async Task TestSetEmailConfirmed()
        {
            await _store.SetEmailConfirmedAsync(_user, true, new CancellationToken());
        }

        [TestMethod]
        public async Task TestSetNormalizedEmail()
        {
            await _store.SetNormalizedEmailAsync(_user, "USER@EXAMPLE.COM", new CancellationToken());
        }

        [TestMethod]
        public async Task TestSetNormalizedUserName()
        {
            await _store.SetNormalizedUserNameAsync(_user, "TESTUSER", new CancellationToken());
        }

        [TestMethod]
        public async Task TestSetPasswordHash()
        {
            await _store.SetPasswordHashAsync(_user, "TESTUSER", new CancellationToken());
        }

        [TestMethod]
        public async Task TestSetUserName()
        {
            await _store.SetUserNameAsync(_user, "TestUser", new CancellationToken());
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            IdentityResult Result = _store.DeleteAsync(_user, new CancellationToken()).Result;
        }
    }
}
