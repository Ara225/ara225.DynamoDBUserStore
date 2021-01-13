using Microsoft.AspNetCore.Identity;
using System;
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
        private AmazonDynamoDBClient _client;
        private DynamoDBContext _context;
        private DynamoDBOperationConfig _userStoreDBConfig = new DynamoDBOperationConfig();
        private DynamoDBOperationConfig _roleStoreDBConfig = new DynamoDBOperationConfig();

        public DynamoDBDataAccessLayer(AmazonDynamoDBClient Client, string DynamoDBUsersTableName, string DynamoDBRolesTableName)
        {
            if (DynamoDBUsersTableName != null)
            {
                _userStoreDBConfig = new DynamoDBOperationConfig { OverrideTableName = DynamoDBUsersTableName };
            }
            if (DynamoDBRolesTableName != null)
            {
                _roleStoreDBConfig = new DynamoDBOperationConfig { OverrideTableName = DynamoDBRolesTableName }; 
            }
            _client = Client;
            _context = new DynamoDBContext(Client); 
        }

        public async Task<bool> SaveUserToDB(DynamoDBUser User, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(User, _userStoreDBConfig, cancellationToken);
            return true;
        }

        public async Task<bool> SaveRoleToDB(DynamoDBRole Role, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(Role, _roleStoreDBConfig, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteUser(DynamoDBUser User, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(User, _userStoreDBConfig, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteRole(DynamoDBRole Role, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(Role, _roleStoreDBConfig, cancellationToken);
            return true;
        }

        public async Task<DynamoDBUser> GetUserById(string Id, CancellationToken cancellationToken)
        {
            return await _context.LoadAsync<DynamoDBUser>(Id, _userStoreDBConfig, cancellationToken);
        }

        public async Task<DynamoDBUser> GetUserByAttribute(string Key, string ExpectedValue, CancellationToken cancellationToken)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition(Key, ScanOperator.Equal, ExpectedValue));
            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync(cancellationToken);
			return UsersList.FirstOrDefault();
        }

        public async Task<DynamoDBUser> GetUserByLogin(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition("LoginProviders", ScanOperator.Contains, loginProvider));
            ConditionList.Add(new ScanCondition("LoginProviderKeys", ScanOperator.Contains, providerKey));

            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync(cancellationToken);
            return UsersList.FirstOrDefault();
        }

        public async Task<List<DynamoDBUser>> GetUsersByClaim(Claim claim, CancellationToken cancellationToken)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition("ClaimTypes", ScanOperator.Contains, claim.Type));
            ConditionList.Add(new ScanCondition("ClaimValues", ScanOperator.Contains, claim.Value));

            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync(cancellationToken);
            return UsersList;
        }

        public async Task<List<DynamoDBUser>> GetUsersByRole(string roleName, CancellationToken cancellationToken)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition("Roles", ScanOperator.Contains, roleName));

            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList, _userStoreDBConfig
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync(cancellationToken);
            return UsersList;
        }

        public async Task<DynamoDBRole> GetRoleById(string Id, CancellationToken cancellationToken)
        {
            return await _context.LoadAsync<DynamoDBRole>(Id, _roleStoreDBConfig, cancellationToken);
        }

        public async Task<DynamoDBRole> GetRoleByName(string NormalizedName, CancellationToken cancellationToken)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition("NormalizedName", ScanOperator.Equal, NormalizedName));
            AsyncSearch<DynamoDBRole> Roles = _context.ScanAsync<DynamoDBRole>(
                ConditionList, _roleStoreDBConfig
            );
            List<DynamoDBRole> RolesList = await Roles.GetRemainingAsync(cancellationToken);
            return RolesList.FirstOrDefault();
        }

    }
}