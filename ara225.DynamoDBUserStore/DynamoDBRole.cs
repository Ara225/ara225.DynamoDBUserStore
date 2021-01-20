/***
 * Data modal for roles
 */
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace ara225.DynamoDBUserStore
{
    [DynamoDBTable(Defaults.DefaultRolesTableName)]
    public class DynamoDBRole 
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamoDBRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initialize the class with a name
        /// </summary>
        /// <param name="roleName">The name for the role</param>
        public DynamoDBRole(string roleName) : this()
        {
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            Name = roleName;
            NormalizedName = roleName.ToUpper();
        }

        /// <summary>
        /// An ID uniquely identifying the role
        /// </summary>
        [DynamoDBHashKey]
        public string Id { get; set; }

        /// <summary>
        /// The role's friendly name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The role's friendly name in upppercase
        /// </summary>
        public string NormalizedName { get; set; }

        /// <summary>
        /// A list of claim types - difficult to store objects in DynamoDB, so condense claims to strings
        /// </summary>
        public List<string> ClaimTypes { get; set; } = new List<string>();

        /// <summary>
        /// A list of claim values - difficult to store objects in DynamoDB, so condense claims to strings
        /// </summary>
        public List<string> ClaimValues { get; set; } = new List<string>();
    }
}
