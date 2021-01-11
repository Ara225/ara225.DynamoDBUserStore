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
	internal static class Defaults
	{
		public const string DefaultUsersTableName = "users";
		public const string DefaultRolesTableName = "roles";
	}
}
