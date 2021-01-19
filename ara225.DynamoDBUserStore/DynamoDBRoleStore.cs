/**
 * Role store implementation
 */
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBRoleStore<TRole> : 
        IRoleStore<DynamoDBRole>, 
        IRoleClaimStore<DynamoDBRole>
        where TRole : DynamoDBRole
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBRoleStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        /// <summary>
        /// Add a claim to a role
        /// </summary>
        /// <param name="role">The role object</param>
        /// <param name="claim">The claim object</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task</returns>
        public Task AddClaimAsync(DynamoDBRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                role.ClaimTypes.Add(claim.Type);
                role.ClaimValues.Add(claim.Value);
            });
        }

        /// <summary>
        /// Save a role to the database for the first time.
        /// </summary>
        /// <param name="role">The role object</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping an IdentityResult representing the success of the operation</returns>
        public async Task<IdentityResult> CreateAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveRoleToDB(role, cancellationToken);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="role">The role object to delete</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping an IdentityResult representing the success of the operation</returns>
        public async Task<IdentityResult> DeleteAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.DeleteRole(role, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Find a role by ID.
        /// </summary>
        /// <param name="roleId">ID to search by</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping a DynamoDBRole or null</returns>
        public async Task<DynamoDBRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetRoleById(roleId, cancellationToken);
        }

        /// <summary>
        /// Find a role by it's normalized name
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping a DynamoDBRole or null</returns>
        public async Task<DynamoDBRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetRoleByName(normalizedRoleName, cancellationToken);
        }

        /// <summary>
        /// Get the claims attached to a role.
        /// </summary>
        /// <param name="role">The role in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping a IList of claims</returns>
        public Task<IList<Claim>> GetClaimsAsync(DynamoDBRole role, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                IList<Claim> Claims = new List<Claim>();
                for (int i = 0; i < role.ClaimTypes.Count; i++)
                {
                    Claims.Add(new Claim(role.ClaimTypes[i], role.ClaimValues[i]));
                }
                return Claims;
            });
        }

        /// <summary>
        /// Get the role's normalized name attribute
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping the role's normalized name</returns>
        public Task<string> GetNormalizedRoleNameAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return role.NormalizedName;
            });
        }

        /// <summary>
        /// Get the role's ID attribute
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping the role's ID</returns>
        public Task<string> GetRoleIdAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return role.Id;
            });
        }

        /// <summary>
        /// Get the role's name attribute
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping the role's name</returns>
        public Task<string> GetRoleNameAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return role.Name;
            });
        }

        /// <summary>
        /// Remove a claim from a role.
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task</returns>
        public Task RemoveClaimAsync(DynamoDBRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                int index = role.ClaimTypes.IndexOf(claim.Type);
                role.ClaimTypes.Remove(claim.Type);
                role.ClaimValues.RemoveAt(index);
            });
        }

        /// <summary>
        /// Set a role's normalized name
        /// </summary>
        /// <param name="role">The role</param>
        /// <param name="normalizedName">The desired normalized name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task</returns>
        public Task SetNormalizedRoleNameAsync(DynamoDBRole role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                role.NormalizedName = normalizedName;
            });
        }

        /// <summary>
        /// Set a role's normalized name.
        /// </summary>
        /// <param name="role">The role in question</param>
        /// <param name="roleName">The desired role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task</returns>
        public Task SetRoleNameAsync(DynamoDBRole role, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                role.Name = roleName;
            });
        }

        /// <summary>
        /// Persist changes to a role to the database.
        /// </summary>
        /// <param name="role">The role in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A Task wrapping an IdentityResult</returns>
        public async Task<IdentityResult> UpdateAsync(DynamoDBRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveRoleToDB(role, cancellationToken);
            return IdentityResult.Success;
        }
    }
}