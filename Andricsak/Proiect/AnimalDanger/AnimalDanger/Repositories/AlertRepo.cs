using AnimalDangerApi.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDangerApi.Repositories
{
    public class AlertRepo
    {
        private CloudTable alertsTable;
        public AlertRepo()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=animaldanger;AccountKey=eu+i0X92PnCCcrPXYdEfzo/onHrgZheCxZY3xsO5mj4Amm1zwF2bEIYpVY7aknILcZETcysR5hDHtN3qokhoXg==;EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = account.CreateCloudTableClient();
            alertsTable = tableClient.GetTableReference("Alerts");
            alertsTable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<Alert>> GetAllAlerts()
        {
            if (alertsTable == null)
            {
                throw new Exception();
            }

            var animals = new List<Alert>();
            TableQuery<Alert> query = new TableQuery<Alert>(); //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Alert> resultSegment = await alertsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                animals.AddRange(resultSegment.Results);
            } while (token != null);

            return animals;
        }

        public async Task<TableResult> InsertOrUpdate(Alert alert)
        {
            if (alertsTable == null)
            {
                Console.WriteLine("Table doesn't exist!");
                throw new Exception();
            }
            var insertOperation = TableOperation.InsertOrMerge(alert);
            return await alertsTable.ExecuteAsync(insertOperation);
        }

        public async Task<Alert> RetrieveSingleEntity(string partitionKey,string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Alert>(partitionKey,rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await alertsTable.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                Console.WriteLine("Result was not null");
                Console.WriteLine(((Alert)retrievedResult.Result).City);
                var retrievedAlert = (Alert)retrievedResult.Result;
                return retrievedAlert;
            }
            else
            {
                Console.WriteLine("The Name could not be retrieved.");
                throw new Exception();
            }
        }
    }
}
