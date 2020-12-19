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

        public async Task<bool> GetTwoFactorEnabledAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.TwoFactorEnabled;
        }

        public async Task SetTwoFactorEnabledAsync(DynamoDBUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
        }

        public async Task<string> GetAuthenticatorKeyAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.AuthenticatorKey;
        }

        public async Task SetAuthenticatorKeyAsync(DynamoDBUser user, string key, CancellationToken cancellationToken)
        {
            user.AuthenticatorKey = key;
        }

        public async Task<string> GetPhoneNumberAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.PhoneNumberConfirmed;
        }

        public async Task SetPhoneNumberAsync(DynamoDBUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
        }

        public async Task SetPhoneNumberConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
        }

        public async Task AddLoginAsync(DynamoDBUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.LoginProviderDisplayNames.Add(login.ProviderDisplayName);
            user.LoginProviderKeys.Add(login.ProviderKey);
            user.LoginProviders.Add(login.LoginProvider);
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

        public async Task<string> GetEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            List<UserLoginInfo> UserLogins = new List<UserLoginInfo>();
            for (int i = 0; i < user.LoginProviders.Count; i++)
            {
                UserLogins.Add(new UserLoginInfo(user.LoginProviders[i], user.LoginProviderKeys[i], user.LoginProviderDisplayNames[i]));
            }
            return UserLogins;
        }

        public async Task<string> GetNormalizedEmailAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task<string> GetNormalizedUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedUserName;
        }

        public async Task<string> GetPasswordHashAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<string> GetUserIdAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }

        public async Task<bool> HasPasswordAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user.PasswordHash == null || user.PasswordHash.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task RemoveLoginAsync(DynamoDBUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
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
        }

        public async Task SetEmailAsync(DynamoDBUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
        }

        public async Task SetEmailConfirmedAsync(DynamoDBUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
        }

        public async Task SetNormalizedEmailAsync(DynamoDBUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
        }

        public async Task SetNormalizedUserNameAsync(DynamoDBUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
        }

        public async Task SetPasswordHashAsync(DynamoDBUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PasswordHash = passwordHash;
        }

        public async Task SetUserNameAsync(DynamoDBUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            user.NormalizedUserName = userName.ToUpper();
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

        public async Task ReplaceCodesAsync(DynamoDBUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            user.RecoveryCodes = recoveryCodes.ToList();
        }

        public async Task<bool> RedeemCodeAsync(DynamoDBUser user, string code, CancellationToken cancellationToken)
        {
            return user.RecoveryCodes.Remove(code);
        }

        public async Task<int> CountCodesAsync(DynamoDBUser user, CancellationToken cancellationToken)
        {
            if (user.RecoveryCodes != null)
            {
                return user.RecoveryCodes.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}