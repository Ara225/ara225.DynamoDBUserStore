using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ara225.DynamoDBUserStore
{
    [DynamoDBTable("DevProjUsersTable")]
    public class DynamoDBUser : IdentityUser
    {
        //
        // Summary:
        //     Initializes a new instance of the class
        public DynamoDBUser()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
            ConcurrencyStamp = Guid.NewGuid().ToString();
            LoginProviders = new List<string>();
            LoginProviderKeys = new List<string>();
            LoginProviderDisplayNames = new List<string>();
            AccessFailedCount = 0;
        }

        //
        // Summary:
        //     Initializes a new instance of the class
        //
        // Parameters:
        //   userName:
        //     The user name.
        public DynamoDBUser(string userName) : this()
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
            NormalizedUserName = userName.ToUpper();
            CreatedOn = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        //
        // Summary:
        //     Gets or sets the primary key for this user.
        [PersonalData]
        [DynamoDBHashKey]
        override public string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the Authenticator key for this user.
        [PersonalData]
        public string AuthenticatorKey { get; set; }
        public string NormalizedUserName { get; set; }

        //
        // Summary:
        //     Gets or sets the date the user was created
        public string CreatedOn { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string NormalizedEmail { get; set; }


        //
        // Summary:
        //     The login providers used by the current user
        public List<string> LoginProviders { get; set; }

        //
        // Summary:
        //     The login provider keys linked to the current user
        public List<string> LoginProviderKeys { get; set; }

        //
        // Summary:
        //     The display name of the login providers used by the current user
        public List<string> LoginProviderDisplayNames { get; set; }

        //
        // Summary:
        //     URL to profile image
        public string ProfileImageURL { get; set; }

        //
        // Summary:
        //     Two factor recovery codes
        public List<string> RecoveryCodes { get; set; }

        [DynamoDBProperty(typeof(DateTimeConverter))]
        public override DateTime? LockoutEndDateUtc { get; set; }

        //
        // Summary:
        //     Returns the username for this user.
        public override string ToString()
        {
            return UserName;
        }
    }
}
