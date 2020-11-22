using AzureB2CDemoApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzureB2CDemoApp.Data
{
    public class UserDAO
    {
        private CosmosDBOptions _cosmosDBOptions;
        public UserDAO(CosmosDBOptions cosmosDBOptions)
        {
            _cosmosDBOptions = cosmosDBOptions;
        }
        public  async Task<HttpStatusCode> CreateUser(ClaimsPrincipal user, string deskId)
        {
            UserInfo userInfoObj = new UserInfo()
            {
                email = user.FindFirst("emails").Value,
                firstName = user.FindFirst(ClaimTypes.GivenName).Value,
                lastName = user.FindFirst(ClaimTypes.Surname).Value,
                id = user.FindFirst(ClaimTypes.NameIdentifier).Value,
                deskId = deskId
            };
            //returnMessage = JsonConvert.SerializeObject(userInfoObj);
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(_cosmosDBOptions.DatabaseEndpoint, _cosmosDBOptions.SecretKey);
            Microsoft.Azure.Cosmos.Database database = await client.CreateDatabaseIfNotExistsAsync(_cosmosDBOptions.DatabaseName);
            Microsoft.Azure.Cosmos.Container container = await database.CreateContainerIfNotExistsAsync(
                "users",
                "/email",
                400);
            // Upsert an item
            Microsoft.Azure.Cosmos.ItemResponse<UserInfo> createResponse = await container.UpsertItemAsync(userInfoObj);
            return createResponse.StatusCode;
        }
    }
}
