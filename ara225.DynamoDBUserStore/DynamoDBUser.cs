using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ara225.DynamoDBUserStore
{
    [DynamoDBTable(Defaults.DefaultUsersTableName)]
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
            AccessFailedCount = 0;
            LockoutEnd = null;
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
        public List<string> LoginProviders { get; set; } = new List<string>();

        //
        // Summary:
        //     The login provider keys linked to the current user
        public List<string> LoginProviderKeys { get; set; } = new List<string>();

        //
        // Summary:
        //     The display name of the login providers used by the current user
        public List<string> LoginProviderDisplayNames { get; set; } = new List<string>();

        //
        // Summary:
        //     Two factor recovery codes
        public List<string> RecoveryCodes { get; set; } = new List<string>();

        [DynamoDBProperty(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? LockoutEnd { get; set; }

        [DynamoDBIgnore]
        public override DateTime? LockoutEndDateUtc { get; set; }

        [DynamoDBIgnore]
        public override ICollection<IdentityUserClaim> Claims { get; }

        [DynamoDBIgnore]
        public override ICollection<IdentityUserLogin> Logins { get; }

        public List<string> ClaimTypes { get; set; } = new List<string>();
        public List<string> ClaimValues { get; set; } = new List<string>();

        public new List<string> Roles { get; set; } = new List<string>();

        public List<string> TokenLoginProviders { get; set; } = new List<string>();
        public List<string> TokenNames { get; set; } = new List<string>();
        public List<string> TokenValues { get; set; } = new List<string>();
    }
}
