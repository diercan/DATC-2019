using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace WebJob
{
    class Program
    {
        private static string storageConnectionString = "DefaultEndpointsProtocol=https;"
                            + "AccountName=datcdemoluni"
                            + ";AccountKey=0iC24GOBAlLYUmGebdyEcmrMdxAMvwtKkLmfNy4mjF7dpigvoXGMU2VSWxEpDUXi5H3czl3+Z2TAYaqpY0nAhw=="
                            + ";EndpointSuffix=core.windows.net";
        static void Main(string[] args)
        {
            Task.Run( async () => await initialize()).GetAwaiter().GetResult();
        }

        static async Task initialize()
        {
            var account = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = account.CreateCloudTableClient();

            CloudTable studentsTable = tableClient.GetTableReference("StudentiAA");
            await CreatePeopleTableAsync(studentsTable);
            await getCount(studentsTable);
        }

        static async Task getCount(CloudTable studentsTable)
        {
            int count = 0;
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    count++;
                }
            } while (token != null);

            Console.WriteLine("Number of students: " + count);
        }

        static public async Task CreatePeopleTableAsync(CloudTable metricsTable)
        {
            // Create the CloudTable if it does not exist
            await metricsTable.CreateIfNotExistsAsync();
        }
    }
}
