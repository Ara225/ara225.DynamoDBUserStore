using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBUserStore<TUser> : 
        IUserTwoFactorRecoveryCodeStore<DynamoDBUser>, 
        IUserTwoFactorStore<DynamoDBUser>, 
        IUserAuthenticatorKeyStore<DynamoDBUser>, 
        IUserPhoneNumberStore<DynamoDBUser>, 
        IUserPasswordStore<DynamoDBUser>, 
        IUserEmailStore<DynamoDBUser>, 
        IUserLoginStore<DynamoDBUser>,
        IUserClaimStore<DynamoDBUser>,
        IUserSecurityStampStore<DynamoDBUser>,
        IUserLockoutStore<DynamoDBUser>,
        IUserRoleStore<DynamoDBUser>,
        IUserStore<DynamoDBUser>,
        IUserAuthenticationTokenStore<DynamoDBUser>
        where TUser : DynamoDBUser
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBUserStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        /// <summary>
        /// Get whether two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.TwoFactorEnabled; 
            });
        }

        /// <summary>
        /// Enabled or disable two factor auth on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled">Bool representing whether the two factor auth is to be enabled</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(DynamoDBUser user, bool enabled, CancellationToken cancellationToken)
        {
             return Task.Run(() => 
             {
                 cancellationToken.ThrowIfCancellationRequested();
                 user.TwoFactorEnabled = enabled; 
             });
        }

        /// <summary>
        /// Get the authenticator key
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetAuthenticatorKeyAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.AuthenticatorKey; 
            });
        }

        /// <summary>
        /// Set the authenticator key
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key">The authenticator key</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetAuthenticatorKeyAsync(DynamoDBUser user, string key, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.AuthenticatorKey = key; 
            });
        }

        /// <summary>
        /// Get the user's phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.PhoneNumber; 
            });
        }

        /// <summary>
        /// Get whether the phone number is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.PhoneNumberConfirmed; 
            });
        }

        /// <summary>
        /// Set a user's phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber">The desired phone number</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(DynamoDBUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.PhoneNumber = phoneNumber; 
            });
        }

        /// <summary>
        /// Set whether the user's phone number is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed">Whether the phone number is confirmed</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.PhoneNumberConfirmed = confirmed; 
            });
        }

        /// <summary>
        /// Add a social login to a user. This involves breaking it into strings for DynamoDB 
        /// compatibility
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login">The desired login</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddLoginAsync(DynamoDBUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < user.LoginProviders.Count; i++)
                {
                    if (user.LoginProviders[i] == login.LoginProvider && user.LoginProviderKeys[i] == login.ProviderKey && user.LoginProviderDisplayNames[i] == login.LoginProvider)
                    {
                        return;
                    }
                }
                user.LoginProviderDisplayNames.Add(login.ProviderDisplayName);
                user.LoginProviderKeys.Add(login.ProviderKey);
                user.LoginProviders.Add(login.LoginProvider);
            });
        }

        /// <summary>
        /// Save a user to theDBfor the first time
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveUserToDB(user, cancellationToken);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.DeleteUser(user, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Find a user by its email
        /// </summary>
        /// <param name="NormalizedEmail">The user's normalized email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> FindByEmailAsync(string NormalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserByAttribute("NormalizedEmail", NormalizedEmail, cancellationToken);
        }

        /// <summary>
        /// Find a user by its Id
        /// </summary>
        /// <param name="Id">The user's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> FindByIdAsync(string Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserById(Id, cancellationToken);
        }

        /// <summary>
        /// Find a user by a social login attached to them
        /// </summary>
        /// <param name="LoginProvider">The name of the login provider</param>
        /// <param name="ProviderKey">A unique key use to identify the user</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserByLogin(LoginProvider, ProviderKey, cancellationToken);
        }

        /// <summary>
        /// Find a user by its normalized username
        /// </summary>
        /// <param name="NormalizedUserName">The user's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> FindByNameAsync(string NormalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserByAttribute("NormalizedUserName", NormalizedUserName, cancellationToken);
        }

        /// <summary>
        /// Get the user's email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.Email; 
            });
        }

        /// <summary>
        /// Get whether the user has confirmed their email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.EmailConfirmed; 
            });
        }

        /// <summary>
        /// Get the user's social logins
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                IList<UserLoginInfo> UserLogins = new List<UserLoginInfo>();
                for (int i = 0; i < user.LoginProviders.Count; i++)
                {
                    UserLogins.Add(new UserLoginInfo(user.LoginProviders[i], user.LoginProviderKeys[i], user.LoginProviderDisplayNames[i]));
                }
                return UserLogins;
            });
        }

        /// <summary>
        /// Get the user's normalized email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.NormalizedEmail; 
            });
        }

        /// <summary>
        /// Get the user's normalized email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.NormalizedUserName; 
            });
        }

        /// <summary>
        /// Get the user's password hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.PasswordHash; 
            });
        }

        /// <summary>
        /// Get the user's ID
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.Id; 
            });
        }

        /// <summary>
        /// Get the user's name
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.UserName; 
            });
        }

        /// <summary>
        /// Get whether the user has a password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (user.PasswordHash == null || user.PasswordHash.Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Remove a social login from a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider">Name of the login provider</param>
        /// <param name="providerKey">Key from the provider unique to the user</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RemoveLoginAsync(DynamoDBUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.SecurityStamp = Guid.NewGuid().ToString();
                for (int i = 0; i < user.LoginProviderKeys.Count; i++)
                {
                    if (user.LoginProviderKeys[i] == providerKey)
                    {
                        user.LoginProviderKeys.RemoveAt(i);
                        user.LoginProviderDisplayNames.RemoveAt(i);
                        user.LoginProviders.RemoveAt(i);
                        break;
                    }
                }
            });
        }

        /// <summary>
        /// Set the user's email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetEmailAsync(DynamoDBUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.Email = email; 
            });
        }

        /// <summary>
        /// Set whether the user's email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetEmailConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.EmailConfirmed = confirmed; 
            });
        }

        /// <summary>
        /// Set a user's normalized email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedEmail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedEmailAsync(DynamoDBUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.NormalizedEmail = normalizedEmail; 
            });
        }

        /// <summary>
        /// Set a user's normalized username
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(DynamoDBUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.NormalizedUserName = normalizedName; 
            });
        }

        /// <summary>
        /// Set the user's password hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(DynamoDBUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = passwordHash;
            });
        }

        /// <summary>
        /// Set the user's username
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetUserNameAsync(DynamoDBUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.UserName = userName;
                user.NormalizedUserName = userName.ToUpper();
            });
        }

        /// <summary>
        /// Update a user already in the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            IdentityResult Result = IdentityResult.Failed();
            bool UpdateResult = await _dataAccess.SaveUserToDB(user, cancellationToken);
            if (UpdateResult)
            {
                Result = IdentityResult.Success;
            }
            return Result;
        }

        /// <summary>
        /// Replace a user's two factor recovery codes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="recoveryCodes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ReplaceCodesAsync(DynamoDBUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.RecoveryCodes = recoveryCodes.ToList();
            });
        }

        /// <summary>
        /// Redeem a two factor recovery code
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> RedeemCodeAsync(DynamoDBUser user, string code, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.RecoveryCodes.Remove(code); 
            });
        }

        /// <summary>
        /// Get the number of two factor auth recovery codes the user has
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> CountCodesAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (user.RecoveryCodes != null)
                {
                    return user.RecoveryCodes.Count;
                }
                else
                {
                    return 0;
                }
            });
        }

        /// <summary>
        /// Get the claims attached to a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                IList<Claim> Claims = new List<Claim>();
                for (int i = 0; i < user.ClaimTypes.Count; i++)
                {
                    Claims.Add(new Claim(user.ClaimTypes[i], user.ClaimValues[i]));
                }
                return Claims;
            });
        }

        /// <summary>
        /// Apply a claim to a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claims"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddClaimsAsync(DynamoDBUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (Claim claim in claims)
                {
                    user.ClaimTypes.Add(claim.Type);
                    user.ClaimValues.Add(claim.Value);
                }
            });
        }

        /// <summary>
        /// Replace a claim
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claim"></param>
        /// <param name="newClaim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ReplaceClaimAsync(DynamoDBUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                RemoveClaimsAsync(user, new Claim[] { claim }, cancellationToken);
                AddClaimsAsync(user, new Claim[] { newClaim }, cancellationToken);
            });
        }

        /// <summary>
        /// Remove all the claims in the list from the user's list of claims
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claims"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RemoveClaimsAsync(DynamoDBUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (Claim claim in claims)
                {
                    int index = user.ClaimTypes.IndexOf(claim.Type);
                    user.ClaimTypes.Remove(claim.Type);
                    user.ClaimValues.RemoveAt(index);
                }
            });
        }

        /// <summary>
        /// Get users who have the supplied claim
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<DynamoDBUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUsersByClaim(claim, cancellationToken);
        }

        /// <summary>
        /// Set the user's security stamp, a stamp designed to be changed whenever the 
        /// user's auth info changes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(DynamoDBUser user, string stamp, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.SecurityStamp = stamp; 
            });
        }

        /// <summary>
        /// Get the user's security stamp, a stamp designed to be changed whenever the 
        /// user's auth info changes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.SecurityStamp; 
            });
        }

        /// <summary>
        /// Get the user's lockout end date
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
  
                return (DateTimeOffset?)user.LockoutEnd;
            });
        }

        /// <summary>
        /// Set the user's lockout end date
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(DynamoDBUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (lockoutEnd.HasValue)
                {
                    user.LockoutEnd = lockoutEnd.Value;
                }
                else
                {
                    user.LockoutEnd = null;
                }
            });
        }

        /// <summary>
        /// Increment the failed login count
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.AccessFailedCount = user.AccessFailedCount + 1;
                return user.AccessFailedCount;
            });
        }

        /// <summary>
        /// Reset the access failed counter
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.AccessFailedCount = 0;
            });
        }

        /// <summary>
        /// Get the access failed counter
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.AccessFailedCount;
            });
        }

        /// <summary>
        /// Get whether lockout is enabled for this user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.LockoutEnabled;
            });
        }

        /// <summary>
        /// Set whether lockout is enabled for this user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(DynamoDBUser user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.LockoutEnabled = enabled;
            });
        }

        /// <summary>
        /// Add a user to a role. 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName">Should be the normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddToRoleAsync(DynamoDBUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (user.Roles.Any(item => { return item == roleName; }))
                {
                    return;
                }
                else
                {
                    user.Roles.Add(roleName);
                }
            });
        }

        /// <summary>
        /// Remove a user from a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName">Should be the normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(DynamoDBUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.Roles.Remove(roleName);
            });
        }

        /// <summary>
        /// Get the list of roles the user is in
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return (IList<string>)user.Roles;
            });
        }

        /// <summary>
        /// Get whether the user is in a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName">Should be the normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> IsInRoleAsync(DynamoDBUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return user.Roles.Contains(roleName);
            });
        }

        /// <summary>
        /// Get users in the role
        /// </summary>
        /// <param name="roleName">Should be the normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<DynamoDBUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _dataAccess.GetUsersByRole(roleName, cancellationToken);
        }

        /// <summary>
        /// Add an authentication token to the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetTokenAsync(DynamoDBUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < user.TokenValues.Count; i++)
                {
                    if (user.TokenLoginProviders[i] == loginProvider && user.TokenNames[i] == name)
                    {
                        user.TokenValues[i] = value;
                        return;
                    }
                }
                user.TokenLoginProviders.Add(loginProvider);
                user.TokenNames.Add(name);
                user.TokenValues.Add(value);
            });
        }

        /// <summary>
        /// Remove an authentication token from a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RemoveTokenAsync(DynamoDBUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < user.TokenValues.Count; i++)
                {
                    if (user.TokenLoginProviders[i] == loginProvider && user.TokenNames[i] == name)
                    {
                        user.TokenLoginProviders.RemoveAt(i);
                        user.TokenNames.RemoveAt(i);
                        user.TokenValues.RemoveAt(i);
                    }
                }
            });
        }

        /// <summary>
        /// Get the value of a authentication token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetTokenAsync(DynamoDBUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < user.TokenValues.Count; i++)
                {
                    if (user.TokenLoginProviders[i] == loginProvider && user.TokenNames[i] == name)
                    {
                        return user.TokenValues[i];
                    }
                }
                return null;
            });
        }
    }
}