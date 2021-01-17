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
    [DynamoDBTable(Defaults.DefaultRolesTableName)]
    public class DynamoDBRole 
    {
        //
        // Summary:
        //     Initializes a new instance of the class
        public DynamoDBRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        //
        // Summary:
        //     Initializes a new instance of the class
        //
        // Parameters:
        //   roleName:
        //     The role name.
        public DynamoDBRole(string roleName) : this()
        {
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            Name = roleName;
            NormalizedName = roleName.ToUpper();
        }

        //
        // Summary:
        //     Gets or sets the primary key for this user.
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public List<string> ClaimTypes { get; set; } = new List<string>();
        public List<string> ClaimValues { get; set; } = new List<string>();
    }
}
