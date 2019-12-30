using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;


namespace Andricsak
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Task.Run( async () => await initialize()).GetAwaiter().GetResult();
        }

        static async Task initialize()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(projectName,m_accountKey),true);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable studentsTable = tableClient.GetTableReference("StudentiAA");    
            //await CreatePeopleTableAsync(studentsTable);

            // StudentEntity student = new StudentEntity("UPT-AC", "12345678");
            // //await insertIntoTableAsync(studentsTable,student);

            StudentEntity student1 = new StudentEntity("UPT-AC","2345678");
            student1.Name="Gheorghe";
            //await insertIntoTableAsync(studentsTable,student1);
            //await GetSingleEntityAsync(studentsTable);
            await GetAllEntitiesAsync(studentsTable);
            await UpdateEntityAsync(studentsTable,student1.PartitionKey,student1.RowKey);
            await GetAllEntitiesAsync(studentsTable);
            //await DeleteEnetityAsync(studentsTable,student1.PartitionKey,student1.RowKey);
            
            
        }

        static async Task insertIntoTableAsync(CloudTable studentsTable,StudentEntity student)
        {
            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(student);

            // Execute the insert operation.
            await studentsTable.ExecuteAsync(insertOperation);
        }
        static public async Task CreatePeopleTableAsync(CloudTable studentsTable)
        {
            // Create the CloudTable if it does not exist
            await studentsTable.CreateIfNotExistsAsync();
        }

        static async Task GetSingleEntityAsync(CloudTable studentsTable)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>("UPT-AC","12345678");

            // Execute the retrieve operation.
            TableResult retrievedResult = await studentsTable.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                Console.WriteLine("Result was not null");
                Console.WriteLine(((StudentEntity)retrievedResult.Result).Name);
            }
            else
                Console.WriteLine("The Name could not be retrieved.");
        }

        static async Task GetAllEntitiesAsync(CloudTable studentsTable)
        {
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UPT-AC"));

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0}, {1}\t{2}", entity.PartitionKey, entity.RowKey,
                    entity.Name);
                }
            } while (token != null);
        }

        static async Task DeleteEnetityAsync(CloudTable studentsTable,string partitionKey,string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);

            // Execute the operation.
            TableResult retrievedResult = await studentsTable.ExecuteAsync(retrieveOperation);

            // Assign the result to a CustomerEntity object.
            StudentEntity deleteEntity = (StudentEntity)retrievedResult.Result;

            // Create the Delete TableOperation and then execute it.
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                // Execute the operation.
                await studentsTable.ExecuteAsync(deleteOperation);

                Console.WriteLine("Entity deleted.");
            }

            else
                Console.WriteLine("Couldn't delete the entity.");
        }


        static async Task UpdateEntityAsync(CloudTable studentsTable,string partitionKey,string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<StudentEntity>(partitionKey, rowKey);

            // Execute the operation.
            TableResult retrievedResult = await studentsTable.ExecuteAsync(retrieveOperation);

            StudentEntity updateEntity = (StudentEntity)retrievedResult.Result;

            updateEntity.Name="NotGheorghe";
            // Create the Delete TableOperation and then execute it.
            if (updateEntity != null)
            {
                TableOperation updateOperation = TableOperation.Merge(updateEntity);
                await studentsTable.ExecuteAsync(updateOperation);

                Console.WriteLine("Entity updated");
            }
            else{
                Console.WriteLine("Entity not found");
            }
        }
    }
}
