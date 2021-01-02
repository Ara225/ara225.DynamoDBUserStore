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

        public DynamoDBDataAccessLayer(AmazonDynamoDBClient Client)
        {
            _client = Client;
            _context = new DynamoDBContext(Client); 
        }

        public async Task<bool> SaveItemToDB(dynamic Item, CancellationToken cancellationToken)
        {
            await _context.SaveAsync(Item, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteItem(dynamic Item, CancellationToken cancellationToken)
        {
            await _context.DeleteAsync(Item, cancellationToken);
            return true;
        }

        public async Task<DynamoDBUser> GetUserById(string Id)
        {
            return await _context.LoadAsync<DynamoDBUser>(Id);
        }

        public async Task<DynamoDBUser> GetUserByAttribute(string Key, string ExpectedValue)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition(Key, ScanOperator.Equal, ExpectedValue));
            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync();
			return UsersList.FirstOrDefault();
        }

        public async Task<DynamoDBUser> GetUserByLogin(string loginProvider, string providerKey)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition("LoginProviders", ScanOperator.Contains, loginProvider));
            ConditionList.Add(new ScanCondition("LoginProviderKeys", ScanOperator.Contains, providerKey));

            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync();
            return UsersList.FirstOrDefault();
        }

        public async Task<List<DynamoDBUser>> GetUsersByClaim(Claim claim)
        {
            List<ScanCondition> ConditionList = new List<ScanCondition>();
            ConditionList.Add(new ScanCondition("ClaimTypes", ScanOperator.Contains, claim.Type));
            ConditionList.Add(new ScanCondition("ClaimValues", ScanOperator.Contains, claim.Value));

            AsyncSearch<DynamoDBUser> Users = _context.ScanAsync<DynamoDBUser>(
                ConditionList
            );
            List<DynamoDBUser> UsersList = await Users.GetRemainingAsync();
            return UsersList;
        }
    }
}