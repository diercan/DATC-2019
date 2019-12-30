using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalDangerApi.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AnimalDangerApi.Repositories
{
    public class AnimalRepo : IAnimalRepo
    {
        private CloudTable animalsTable;
        public AnimalRepo()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=animaldanger;AccountKey=eu+i0X92PnCCcrPXYdEfzo/onHrgZheCxZY3xsO5mj4Amm1zwF2bEIYpVY7aknILcZETcysR5hDHtN3qokhoXg==;EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = account.CreateCloudTableClient();
            animalsTable = tableClient.GetTableReference("Animals");
        }

        public async Task<IEnumerable<Animal>> GetAllAnimals()
        {
            if (animalsTable == null)
            {
                throw new Exception();
            }

            var animals = new List<Animal>();
            TableQuery<Animal> query = new TableQuery<Animal>(); //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Animal> resultSegment = await animalsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                animals.AddRange(resultSegment.Results);
            } while (token != null);

            return animals;
        }

        public async Task<TableResult> InsertOrUpdate(Animal animal)
        {
            if (animalsTable == null)
            {
                Console.WriteLine("Table doesn't exist!");
                throw new Exception();
            }
            var insertOperation = TableOperation.InsertOrMerge(animal);
            return await animalsTable.ExecuteAsync(insertOperation);
        }

        public async Task<Animal> RetrieveSingleEntity(string id)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Animal>(AnimalType.Bear.ToString(),id);

            // Execute the retrieve operation.
            TableResult retrievedResult = await animalsTable.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                Console.WriteLine("Result was not null");
                Console.WriteLine(((Animal)retrievedResult.Result).Name);
                var retrievedAnimal = (Animal)retrievedResult.Result;
                return retrievedAnimal;
            }
            else
            {
                Console.WriteLine("The Name could not be retrieved.");
                throw new Exception();
            }
        }
    }
}
