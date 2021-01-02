using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBUserStore : 
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
        IUserStore<DynamoDBUser>
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBUserStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        public Task<bool> GetTwoFactorEnabledAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.TwoFactorEnabled; 
            });
        }

        public Task SetTwoFactorEnabledAsync(DynamoDBUser user, bool enabled, CancellationToken cancellationToken)
        {
             return Task.Run(() => 
             {
                 if (user == null)
                 {
                     throw new ArgumentNullException();
                 }

                 cancellationToken.ThrowIfCancellationRequested();
                 user.TwoFactorEnabled = enabled; 
             });
        }

        public Task<string> GetAuthenticatorKeyAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.AuthenticatorKey; 
            });
        }

        public Task SetAuthenticatorKeyAsync(DynamoDBUser user, string key, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (key == null || user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.AuthenticatorKey = key; 
            });
        }

        public Task<string> GetPhoneNumberAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.PhoneNumber; 
            });
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.PhoneNumberConfirmed; 
            });
        }

        public Task SetPhoneNumberAsync(DynamoDBUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || phoneNumber == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.PhoneNumber = phoneNumber; 
            });
        }

        public Task SetPhoneNumberConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.PhoneNumberConfirmed = confirmed; 
            });
        }

        public Task AddLoginAsync(DynamoDBUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || login == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.LoginProviderDisplayNames.Add(login.ProviderDisplayName);
                user.LoginProviderKeys.Add(login.ProviderKey);
                user.LoginProviders.Add(login.LoginProvider);
            });
        }

        public async Task<IdentityResult> CreateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.SaveItemToDB(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _dataAccess.DeleteItem(user, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {

        }

        public async Task<DynamoDBUser> FindByEmailAsync(string NormalizedEmail, CancellationToken cancellationToken)
        {
            if (NormalizedEmail == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();

            return await _dataAccess.GetUserByAttribute("NormalizedEmail", NormalizedEmail);
        }

        public async Task<DynamoDBUser> FindByIdAsync(string Id, CancellationToken cancellationToken)
        {
            if (Id == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserById(Id);
        }

        public async Task<DynamoDBUser> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancellationToken)
        {
            if (LoginProvider == null || ProviderKey == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserByLogin(LoginProvider, ProviderKey);
        }

        public async Task<DynamoDBUser> FindByNameAsync(string NormalizedUserName, CancellationToken cancellationToken)
        {
            if (NormalizedUserName == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUserByAttribute("NormalizedUserName", NormalizedUserName);
        }

        public Task<string> GetEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.Email; 
            });
        }

        public Task<bool> GetEmailConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.EmailConfirmed; 
            });
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                IList<UserLoginInfo> UserLogins = new List<UserLoginInfo>();
                for (int i = 0; i < user.LoginProviders.Count; i++)
                {
                    UserLogins.Add(new UserLoginInfo(user.LoginProviders[i], user.LoginProviderKeys[i], user.LoginProviderDisplayNames[i]));
                }
                return UserLogins;
            });
        }

        public Task<string> GetNormalizedEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.NormalizedEmail; 
            });
        }

        public Task<string> GetNormalizedUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.NormalizedUserName; 
            });
        }

        public Task<string> GetPasswordHashAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.PasswordHash; 
            });
        }

        public Task<string> GetUserIdAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.Id; 
            });
        }

        public Task<string> GetUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.UserName; 
            });
        }

        public Task<bool> HasPasswordAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

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

        public Task RemoveLoginAsync(DynamoDBUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || loginProvider == null || providerKey == null)
                {
                    throw new ArgumentNullException();
                }

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

        public Task SetEmailAsync(DynamoDBUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || email == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.Email = email; 
            });
        }

        public Task SetEmailConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.EmailConfirmed = confirmed; 
            });
        }

        public Task SetNormalizedEmailAsync(DynamoDBUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || normalizedEmail == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.NormalizedEmail = normalizedEmail; 
            });
        }

        public Task SetNormalizedUserNameAsync(DynamoDBUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || normalizedName == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.NormalizedUserName = normalizedName; 
            });
        }

        public Task SetPasswordHashAsync(DynamoDBUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || passwordHash == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = passwordHash;
            });
        }

        public Task SetUserNameAsync(DynamoDBUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || userName == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.UserName = userName;
                user.NormalizedUserName = userName.ToUpper();
            });
        }

        public async Task<IdentityResult> UpdateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            IdentityResult Result = IdentityResult.Failed();
            bool UpdateResult = await _dataAccess.SaveItemToDB(user, cancellationToken);
            if (UpdateResult)
            {
                Result = IdentityResult.Success;
            }
            return Result;
        }

        public Task ReplaceCodesAsync(DynamoDBUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || recoveryCodes == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.RecoveryCodes = recoveryCodes.ToList();
            });
        }

        public Task<bool> RedeemCodeAsync(DynamoDBUser user, string code, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || code == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.RecoveryCodes.Remove(code); 
            });
        }

        public Task<int> CountCodesAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

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

        public Task<IList<Claim>> GetClaimsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                IList<Claim> Claims = new List<Claim>();
                for (int i = 0; i < user.ClaimTypes.Count; i++)
                {
                    Claims.Append(new Claim(user.ClaimTypes[i], user.ClaimValues[i]));
                }
                return Claims;
            });
        }

        public Task AddClaimsAsync(DynamoDBUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || claims == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                foreach (Claim claim in claims)
                {
                    user.ClaimTypes.Add(claim.Type);
                    user.ClaimValues.Add(claim.Value);
                }
            });
        }

        public Task ReplaceClaimAsync(DynamoDBUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || claim == null || newClaim == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                RemoveClaimsAsync(user, new Claim[] { claim }, cancellationToken);
                AddClaimsAsync(user, new Claim[] { claim }, cancellationToken);
            });
        }

        public Task RemoveClaimsAsync(DynamoDBUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null || claims == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                foreach (Claim claim in claims)
                {
                    int index = user.ClaimTypes.IndexOf(claim.Type);
                    user.ClaimTypes.Remove(claim.Type);
                    user.ClaimValues.RemoveAt(index);
                }
            });
        }

        public async Task<IList<DynamoDBUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            if (claim == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.GetUsersByClaim(claim);
        }

        public Task SetSecurityStampAsync(DynamoDBUser user, string stamp, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || stamp == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.SecurityStamp = stamp; 
            });
        }

        public Task<string> GetSecurityStampAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.SecurityStamp; 
            });
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                DateTimeOffset? ReturnValue;
                if (user.LockoutEndDateUtc == null)
                {
                    ReturnValue = null;
                }
                else
                {
                    ReturnValue = DateTimeOffset.Parse(user.LockoutEndDateUtc.ToString());
                }
                return ReturnValue;
            });
        }

        public Task SetLockoutEndDateAsync(DynamoDBUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null || lockoutEnd == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                if (lockoutEnd == null)
                {
                    return;
                }
                else
                {
                    user.LockoutEndDateUtc = lockoutEnd.Value.UtcDateTime;
                }
            });
        }

        public Task<int> IncrementAccessFailedCountAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.AccessFailedCount = user.AccessFailedCount + 1;
                return user.AccessFailedCount;
            });
        }

        public Task ResetAccessFailedCountAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.AccessFailedCount = 0;
            });
        }

        public Task<int> GetAccessFailedCountAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.AccessFailedCount;
            });
        }

        public Task<bool> GetLockoutEnabledAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                return user.LockoutEnabled;
            });
        }

        public Task SetLockoutEnabledAsync(DynamoDBUser user, bool enabled, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                cancellationToken.ThrowIfCancellationRequested();
                user.LockoutEnabled = enabled;
            });
        }

        public Task AddToRoleAsync(DynamoDBUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null || roleName == null)
            {
                throw new ArgumentNullException();
            }

            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(DynamoDBUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(DynamoDBUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DynamoDBUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}