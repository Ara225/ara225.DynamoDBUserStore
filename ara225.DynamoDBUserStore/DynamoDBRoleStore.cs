using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBRoleStore : IRoleStore<DynamoDBRole>, IRoleClaimStore<DynamoDBRole>|
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBRoleStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        public Task AddClaimAsync(DynamoDBRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<DynamoDBRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<DynamoDBRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(DynamoDBRole role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(DynamoDBRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(DynamoDBRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(DynamoDBRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}