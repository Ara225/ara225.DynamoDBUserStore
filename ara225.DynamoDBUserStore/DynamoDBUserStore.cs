using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBUserStore : IUserTwoFactorRecoveryCodeStore<DynamoDBUser>, IUserTwoFactorStore<DynamoDBUser>, IUserAuthenticatorKeyStore<DynamoDBUser>, IUserPhoneNumberStore<DynamoDBUser>, IUserPasswordStore<DynamoDBUser>, IUserEmailStore<DynamoDBUser>, IUserLoginStore<DynamoDBUser>
    {
        private DynamoDBDataAccessLayer _dataAccess;
        public DynamoDBUserStore(DynamoDBDataAccessLayer da)
        {
            _dataAccess = da;
        }

        public Task<bool> GetTwoFactorEnabledAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.TwoFactorEnabled; });
        }

        public Task SetTwoFactorEnabledAsync(DynamoDBUser user, bool enabled, CancellationToken cancellationToken)
        {
             return Task.Run(() => { user.TwoFactorEnabled = enabled; });
        }

        public Task<string> GetAuthenticatorKeyAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.AuthenticatorKey; });
        }

        public Task SetAuthenticatorKeyAsync(DynamoDBUser user, string key, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.AuthenticatorKey = key; });
        }

        public Task<string> GetPhoneNumberAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.PhoneNumber; });
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.PhoneNumberConfirmed; });
        }

        public Task SetPhoneNumberAsync(DynamoDBUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.PhoneNumber = phoneNumber; });
        }

        public Task SetPhoneNumberConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.PhoneNumberConfirmed = confirmed; });
        }

        public Task AddLoginAsync(DynamoDBUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
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
            return await _dataAccess.GetUserByAttribute("NormalizedUserName", NormalizedUserName);
        }

        public Task<string> GetEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.Email; });
        }

        public Task<bool> GetEmailConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.EmailConfirmed; });
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
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
            return Task.Run(() => { return user.NormalizedEmail; });
        }

        public Task<string> GetNormalizedUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.NormalizedUserName; });
        }

        public Task<string> GetPasswordHashAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.PasswordHash; });
        }

        public Task<string> GetUserIdAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.Id; });
        }

        public Task<string> GetUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.UserName; });
        }

        public Task<bool> HasPasswordAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
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
            return Task.Run(() => { user.Email = email; });
        }

        public Task SetEmailConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.EmailConfirmed = confirmed; });
        }

        public Task SetNormalizedEmailAsync(DynamoDBUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.NormalizedEmail = normalizedEmail; });
        }

        public Task SetNormalizedUserNameAsync(DynamoDBUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() => { user.NormalizedUserName = normalizedName; });
        }

        public Task SetPasswordHashAsync(DynamoDBUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = passwordHash;
            });
        }

        public Task SetUserNameAsync(DynamoDBUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                user.UserName = userName;
                user.NormalizedUserName = userName.ToUpper();
            });
        }

        public async Task<IdentityResult> UpdateAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
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
                user.RecoveryCodes = recoveryCodes.ToList();
            });
        }

        public Task<bool> RedeemCodeAsync(DynamoDBUser user, string code, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return user.RecoveryCodes.Remove(code); });
        }

        public Task<int> CountCodesAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
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
    }
}