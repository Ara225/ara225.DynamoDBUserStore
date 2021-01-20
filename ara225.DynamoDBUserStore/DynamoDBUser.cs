/**
 * Data model for users
 */
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
       /// <summary>
       /// Default constructor
       /// </summary>
        public DynamoDBUser()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
            ConcurrencyStamp = Guid.NewGuid().ToString();
            AccessFailedCount = 0;
            LockoutEnd = null;
        }

        /// <summary>
        /// Initialize the class with a username
        /// </summary>
        /// <param name="userName">Desired username</param>
        public DynamoDBUser(string userName) : this()
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
            NormalizedUserName = userName.ToUpper();
        }

        /// <summary>
        /// Gets or sets the primary key for the user, a unique ID
        /// </summary>
        [PersonalData]
        [DynamoDBHashKey]
        override public string Id { get; set; }

        /// <summary>
        ///  Gets or sets the Authenticator key for this user.
        /// </summary>
        [ProtectedPersonalData]
        public string AuthenticatorKey { get; set; }

        /// <summary>
        ///  Gets or sets the normalized username for this user.
        /// </summary>
        [ProtectedPersonalData]  
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// Could be used to prevent concurrent changes to the same user happening. 
        /// Not used directly by this module
        /// </summary>
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        ///  Gets or sets the normalized email for this user.
        /// </summary>
        [ProtectedPersonalData]
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// The login providers used by the current user
        /// </summary>
        [PersonalData]
        public List<string> LoginProviders { get; set; } = new List<string>();


        /// <summary>
        /// The login provider keys linked to the current user
        /// </summary>
        [ProtectedPersonalData]
        public List<string> LoginProviderKeys { get; set; } = new List<string>();

        /// <summary>
        /// The display name of the login providers used by the current user
        /// </summary>
        [PersonalData]
        public List<string> LoginProviderDisplayNames { get; set; } = new List<string>();

        /// <summary>
        /// Two factor recovery codes for the user
        /// </summary>
        [ProtectedPersonalData]
        public List<string> RecoveryCodes { get; set; } = new List<string>();

        /// <summary>
        /// The date and time the users lockout expires. null or dates in the past are 
        /// treated as ended. The converter is needed because DynamoDB doesn't 
        /// support DateTimeOffsets natively
        /// </summary>
        [DynamoDBProperty(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Property we don't need inherited from IdentityUser
        /// </summary>
        [DynamoDBIgnore]
        public override DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Property we don't need inherited from IdentityUser
        /// </summary>
        [DynamoDBIgnore]
        public override ICollection<IdentityUserClaim> Claims { get; }

        /// <summary>
        /// Property we don't need inherited from IdentityUser
        /// </summary>
        [DynamoDBIgnore]
        public override ICollection<IdentityUserLogin> Logins { get; }

        /// <summary>
        /// A list of claim types - difficult to store objects in DynamoDB, so condense claims to strings
        /// </summary>
        public List<string> ClaimTypes { get; set; } = new List<string>();

        /// <summary>
        /// A list of claim values - difficult to store objects in DynamoDB, so condense claims to strings
        /// </summary>
        public List<string> ClaimValues { get; set; } = new List<string>();

        /// <summary>
        /// A list of role names the use is in. Expected to be normalized names
        /// </summary>
        public new List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// A list of authentication token providers
        /// </summary>
        [PersonalData]
        public List<string> TokenLoginProviders { get; set; } = new List<string>();

        /// <summary>
        /// A list of authentication token names
        /// </summary>
        [PersonalData]
        public List<string> TokenNames { get; set; } = new List<string>();

        /// <summary>
        /// A list of authentication token names
        /// </summary>
        [ProtectedPersonalData]
        public List<string> TokenValues { get; set; } = new List<string>();
    }
}
