using System;
using Xunit;
using Microsoft.AspNetCore.Identity.Test;
using ara225.DynamoDBUserStore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;

namespace DynamoDBUserStoreTests
{
    public class UserManagerUnitTests : UserManagerSpecificationTestBase<DynamoDBUser>
    {
        protected override object CreateTestContext()
        {
            return new object();
        }

        protected override void AddUserStore(IServiceCollection services, object context = null)
        {
            services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient(), "UserStoreTable", "RoleStoreTable"));
            services.AddIdentity<DynamoDBUser, DynamoDBRole>().AddUserStore<DynamoDBUserStore<DynamoDBUser>>();
        }

        protected override DynamoDBUser CreateTestUser(string namePrefix = "", string email = "", string phoneNumber = "", bool lockoutEnabled = false, DateTimeOffset? lockoutEnd = null, bool useNamePrefixAsUserName = false)
        {
            DynamoDBUser User = new DynamoDBUser(useNamePrefixAsUserName ? namePrefix : string.Format("{0}{1}", namePrefix, Guid.NewGuid()));
            User.Email = email;
            User.PhoneNumber = phoneNumber;
            User.LockoutEnabled = lockoutEnabled;
            if (lockoutEnd.HasValue)
            {
                User.LockoutEnd = lockoutEnd.Value;
            }
            return User;
        }

        protected override void SetUserPasswordHash(DynamoDBUser user, string hashedPassword)
        {
            user.PasswordHash = hashedPassword;
        }

        protected override Expression<Func<DynamoDBUser, bool>> UserNameEqualsPredicate(string userName) => u => u.UserName == userName;

        protected override Expression<Func<DynamoDBUser, bool>> UserNameStartsWithPredicate(string userName) => u => u.UserName.StartsWith(userName);
    }
}