using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBRoleStore : IRoleStore<DynamoDBRole>, IRoleClaimStore<DynamoDBRole>
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBRoleStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        public Task AddClaimAsync(DynamoDBRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                if (role == null || claim == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                role.ClaimTypes.Add(claim.Type);
                role.ClaimValues.Add(claim.Value);
            });
        }

        public async Task<IdentityResult> CreateAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveItemToDB(role, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.DeleteItem(role, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<DynamoDBRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (roleId == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetRoleById(roleId, cancellationToken);
        }

        public async Task<DynamoDBRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if (normalizedRoleName == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetRoleByName(normalizedRoleName, cancellationToken);
        }

        public Task<IList<Claim>> GetClaimsAsync(DynamoDBRole role, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                if (role == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                IList<Claim> Claims = new List<Claim>();
                for (int i = 0; i < role.ClaimTypes.Count; i++)
                {
                    Claims.Append(new Claim(role.ClaimTypes[i], role.ClaimValues[i]));
                }
                return Claims;
            });
        }

        public Task<string> GetNormalizedRoleNameAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (role == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return role.NormalizedName;
            });
        }

        public Task<string> GetRoleIdAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (role == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return role.Id;
            });
        }

        public Task<string> GetRoleNameAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (role == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return role.Name;
            });
        }

        public Task RemoveClaimAsync(DynamoDBRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                if (role == null || claim == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                int index = role.ClaimTypes.IndexOf(claim.Type);
                role.ClaimTypes.Remove(claim.Type);
                role.ClaimValues.RemoveAt(index);
            });
        }

        public Task SetNormalizedRoleNameAsync(DynamoDBRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (role == null || normalizedName == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                role.NormalizedName = normalizedName;
            });
        }

        public Task SetRoleNameAsync(DynamoDBRole role, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (role == null || roleName == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                role.Name = roleName;
            });
        }

        public async Task<IdentityResult> UpdateAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveItemToDB(role, cancellationToken);
            return IdentityResult.Success;
        }
    }
}