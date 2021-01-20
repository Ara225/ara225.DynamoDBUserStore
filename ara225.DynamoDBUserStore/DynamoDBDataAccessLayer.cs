/**
 * Code that actually accesses DynamoDB
 */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Threading;
using Amazon.DynamoDBv2.DocumentModel;
using System.Security.Claims;

namespace ara225.DynamoDBUserStore
{
    public class DynamoDBDataAccessLayer
    {
        private DynamoDBContext _context;
        private DynamoDBOperationConfig _userStoreDBConfig = new DynamoDBOperationConfig();
        private DynamoDBOperationConfig _roleStoreDBConfig = new DynamoDBOperationConfig();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="client">An AmazonDynamoDBClient configured as required</param>
        /// <param name="dynamoDBUsersTableName">String representing the name of the user storage table</param>
        /// <param name="dynamoDBRolesTableName">String representing the name of the role storage table</param>
        public DynamoDBDataAccessLayer(AmazonDynamoDBClient client, string dynamoDBUsersTableName, string dynamoDBRolesTableName)
        {
            // Need to override table name so that we target the right table and to set the conversion to 
            // V2. This ensures that DynamoDB converts lists and bools correctly into DyanmoDB lists and bools 
            if (dynamoDBUsersTableName != null)
            {
                _userStoreDBConfig = new DynamoDBOperationConfig { OverrideTableName = dynamoDBUsersTableName, Conversion = DynamoDBEntryConversion.V2 };
            }
            if (dynamoDBRolesTableName != null)
            {
                _roleStoreDBConfig = new DynamoDBOperationConfig { OverrideTableName = dynamoDBRolesTableName, Conversion = DynamoDBEntryConversion.V2 }; 
            }

            DynamoDBContextConfig Config = new DynamoDBContextConfig
            {
                Conversion = DynamoDBEntryConversion.V2
            };
            _context = new DynamoDBContext(client, Config); 
        }

        /// <summary>
        /// Save the user to a DynamoDB table. Works for updates and creation
        /// </summary>
        /// <param name="user">The user in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveUserToDB(DynamoDBUser user, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(user, _userStoreDBConfig, cancellationToken);
            return true;
        }

        /// <summary>
        /// Save a role to a DynamoDB table. Works for updates and creation
        /// </summary>
        /// <param name="role">The role in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveRoleToDB(DynamoDBRole role, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(role, _roleStoreDBConfig, cancellationToken);
            return true;
        }

        /// <summary>
        /// Deletes a user from a DynamoDB table.
        /// </summary>
        /// <param name="user">The user in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(DynamoDBUser user, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(user, _userStoreDBConfig, cancellationToken);
            return true;
        }

        /// <summary>
        /// Deletes a role from a DynamoDB table.
        /// </summary>
        /// <param name="role">The role in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRole(DynamoDBRole role, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(role, _roleStoreDBConfig, cancellationToken);
            return true;
        }

        /// <summary>
        /// Retrieve a single user from the DB
        /// </summary>
        /// <param name="id">The desired user's ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> GetUserById(string id, CancellationToken cancellationToken)
        {
            return await _context.LoadAsync<DynamoDBUser>(id, _userStoreDBConfig, cancellationToken);
        }

        /// <summary>
        /// Search for users by any of their attributes
        /// </summary>
        /// <param name="key">Name of the attribute</param>
        /// <param name="expectedValue">Value that the attribute will have</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> GetUserByAttribute(string key, string expectedValue, CancellationToken cancellationToken)
        {
            List<ScanCondition> conditionList = new List<ScanCondition>();
            conditionList.Add(new ScanCondition(key, ScanOperator.Equal, expectedValue));
            AsyncSearch<DynamoDBUser> users = _context.ScanAsync<DynamoDBUser>(
                conditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> usersList = await users.GetRemainingAsync(cancellationToken);
			return usersList.FirstOrDefault();
        }

        /// <summary>
        /// Get a user by one of the social logins they have used with the site
        /// </summary>
        /// <param name="loginProvider">Name of the provider</param>
        /// <param name="providerKey">Unique key identifying the user</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBUser> GetUserByLogin(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            List<ScanCondition> conditionList = new List<ScanCondition>();
            conditionList.Add(new ScanCondition("LoginProviders", ScanOperator.Contains, loginProvider));
            conditionList.Add(new ScanCondition("LoginProviderKeys", ScanOperator.Contains, providerKey));

            AsyncSearch<DynamoDBUser> users = _context.ScanAsync<DynamoDBUser>(
                conditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> usersList = await users.GetRemainingAsync(cancellationToken);
            return usersList.FirstOrDefault();
        }

        /// <summary>
        /// Get all users with the provided claim
        /// </summary>
        /// <param name="claim">The claim in question</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<DynamoDBUser>> GetUsersByClaim(Claim claim, CancellationToken cancellationToken)
        {
            List<ScanCondition> conditionList = new List<ScanCondition>();
            conditionList.Add(new ScanCondition("ClaimTypes", ScanOperator.Contains, claim.Type));
            conditionList.Add(new ScanCondition("ClaimValues", ScanOperator.Contains, claim.Value));

            AsyncSearch<DynamoDBUser> users = _context.ScanAsync<DynamoDBUser>(
                conditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> usersList = await users.GetRemainingAsync(cancellationToken);
            return usersList;
        }

        /// <summary>
        /// Get users who are in a role.
        /// </summary>
        /// <param name="roleName">Normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<DynamoDBUser>> GetUsersByRole(string roleName, CancellationToken cancellationToken)
        {
            List<ScanCondition> conditionList = new List<ScanCondition>();
            conditionList.Add(new ScanCondition("Roles", ScanOperator.Contains, roleName));

            AsyncSearch<DynamoDBUser> users = _context.ScanAsync<DynamoDBUser>(
                conditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> usersList = await users.GetRemainingAsync(cancellationToken);
            return usersList;
        }

        /// <summary>
        /// Get a role by its ID
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBRole> GetRoleById(string id, CancellationToken cancellationToken)
        {
            return await _context.LoadAsync<DynamoDBRole>(id, _roleStoreDBConfig, cancellationToken);
        }
        /// <summary>
        /// Get a role by its normalized name.
        /// </summary>
        /// <param name="normalizedName">Normalized role name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DynamoDBRole> GetRoleByName(string normalizedName, CancellationToken cancellationToken)
        {
            List<ScanCondition> conditionList = new List<ScanCondition>();
            conditionList.Add(new ScanCondition("NormalizedName", ScanOperator.Equal, normalizedName));
            AsyncSearch<DynamoDBRole> Roles = _context.ScanAsync<DynamoDBRole>(
                conditionList, _roleStoreDBConfig
            );
            List<DynamoDBRole> RolesList = await Roles.GetRemainingAsync(cancellationToken);
            return RolesList.FirstOrDefault();
        }
    }
}