using System;
using Xunit;
using Microsoft.AspNetCore.Identity.Test;
using ara225.DynamoDBUserStore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace DynamoDBUserStoreTests
{
    public class IdentitySpecificationUnitTests : IdentitySpecificationTestBase<DynamoDBUser, DynamoDBRole>
    {
        protected override void AddRoleStore(IServiceCollection services, object context = null)
        {
            throw new NotImplementedException();
        }

        protected override void AddUserStore(IServiceCollection services, object context = null)
        {
            throw new NotImplementedException();
        }

        protected override object CreateTestContext()
        {
            throw new NotImplementedException();
        }

        protected override DynamoDBRole CreateTestRole(string roleNamePrefix = "", bool useRoleNamePrefixAsRoleName = false)
        {
            throw new NotImplementedException();
        }

        protected override DynamoDBUser CreateTestUser(string namePrefix = "", string email = "", string phoneNumber = "", bool lockoutEnabled = false, DateTimeOffset? lockoutEnd = null, bool useNamePrefixAsUserName = false)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<DynamoDBRole, bool>> RoleNameEqualsPredicate(string roleName)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<DynamoDBRole, bool>> RoleNameStartsWithPredicate(string roleName)
        {
            throw new NotImplementedException();
        }

        protected override void SetUserPasswordHash(DynamoDBUser user, string hashedPassword)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<DynamoDBUser, bool>> UserNameEqualsPredicate(string userName)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<DynamoDBUser, bool>> UserNameStartsWithPredicate(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
