using System;
using Xunit;
using Microsoft.AspNetCore.Identity.Test;
using ara225.DynamoDBUserStore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Amazon.DynamoDBv2;

namespace DynamoDBUserStoreTests
{
    public class IdentitySpecificationUnitTests : DynamoDBUserStoreTests.IdentitySpecificationTestBase<DynamoDBUser, DynamoDBRole>
    {

        protected override object CreateTestContext()
        {
            return new object();
        }

        protected override void AddRoleStore(IServiceCollection services, object context = null)
        {
            if (Environment.GetEnvironmentVariable("CONNECT_TO_CLOUD_DB") != null)
            {
                // To test with DynamoDB in the cloud
                services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new Amazon.DynamoDBv2.AmazonDynamoDBClient(), "UserStoreTable", "RoleStoreTable"));
            }
            else
            {
                // To test with a local DynamoDB in Docker
                services.AddSingleton<DynamoDBDataAccessLayer>(x => new DynamoDBDataAccessLayer(new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" }), "UserStoreTable", "RoleStoreTable"));
            }
            services.AddIdentity<DynamoDBUser, DynamoDBRole>()
                .AddUserStore<DynamoDBUserStore<DynamoDBUser>>()
                .AddRoleStore<DynamoDBRoleStore<DynamoDBRole>>();
        }

        protected override void AddUserStore(IServiceCollection services, object context = null)
        {
            return;
        }

        protected override DynamoDBRole CreateTestRole(string roleNamePrefix = "", bool useRoleNamePrefixAsRoleName = false)
        {
            DynamoDBRole Role = new DynamoDBRole(useRoleNamePrefixAsRoleName ? roleNamePrefix : string.Format("{0}{1}", roleNamePrefix, Guid.NewGuid()));
            return Role;
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

        protected override Expression<Func<DynamoDBRole, bool>> RoleNameEqualsPredicate(string roleName) => r => r.Name == roleName;

        protected override Expression<Func<DynamoDBRole, bool>> RoleNameStartsWithPredicate(string roleName) => r => r.Name.StartsWith(roleName);

        protected override void SetUserPasswordHash(DynamoDBUser user, string hashedPassword)
        {
            user.PasswordHash = hashedPassword;
        }

        protected override Expression<Func<DynamoDBUser, bool>> UserNameEqualsPredicate(string userName) => u => u.UserName == userName;

        protected override Expression<Func<DynamoDBUser, bool>> UserNameStartsWithPredicate(string userName) => u => u.UserName.StartsWith(userName);
    }
}
