using System.Linq;
using AzureFunction.Models;
using AzureFunction.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DATC.AzureFunction
{
    public static class ServiceBusQueueTriggerCSharp
    {
        private const string TableStorageConnectionString = "";
        private const string MyValuesTableName = "MyValuesTable";
        private const string MySumTableName = "MySumTable";

        [FunctionName("ServiceBusQueueTriggerCSharp")]
        public static void Run([ServiceBusTrigger("myqueue-1", Connection = "ConnectionStringKey")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var myValuesRepository = new TableStorageService<MyValue>(log, TableStorageConnectionString, MyValuesTableName);
            var mySumRepository = new TableStorageService<MyValue>(log, TableStorageConnectionString, MySumTableName);

            var myValue = JsonConvert.DeserializeObject<MyValue>(myQueueItem);
            myValuesRepository.InsertOrUpdateEntry(myValue);

            // this is not memory efficient
            var latestValues = myValuesRepository.GetAllEntries().OrderBy(v => v.Timestamp).TakeLast(10);
            var totalSum = latestValues.Sum(v => v.Value);

            var myNewestSum = new MyValue()
            {
                Value = totalSum
            };

            mySumRepository.InsertOrUpdateEntry(myNewestSum);
        }
    }
}
