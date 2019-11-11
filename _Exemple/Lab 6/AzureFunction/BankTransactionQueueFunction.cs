using AzureFunction.Models;
using AzureFunction.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AzureFunction
{
    public static class BankTransactionQueueFunction
    {
        public const string QUEUE_NAME = "myqueue-1";
        
        public const string TABLE_STORAGE_CONNECTION_STRING = "";
        //public const string SERVICE_BUS_CONNECTION_STRING = "";

        public const string MY_NAME = "AlexandruRus";

        [FunctionName("BankTransactionQueueFunction")]
        public static void Run([ServiceBusTrigger(QUEUE_NAME, Connection = "ConnectionStringKey")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var bankTransactionRepository = new TableStorageService<BankTransaction>(log, TABLE_STORAGE_CONNECTION_STRING, $"{MY_NAME}BankTransaction");
            var bankTransactionStatisticsRepository = new TableStorageService<BankTransactionStatistics>(log, TABLE_STORAGE_CONNECTION_STRING, $"{MY_NAME}BankTransactionStatistics");

            var queueMessage = JsonConvert.DeserializeObject<QueueMessage>(myQueueItem);
            var transaction = new BankTransaction(queueMessage);            

            bankTransactionRepository.InsertOrUpdateEntry(transaction);

            var transactions = bankTransactionRepository.GetByPartitionId(transaction.PartitionKey);
            var totalAmmount = transactions.Sum(t => t.Ammount);

            var existingBankTransactionStatistics = bankTransactionStatisticsRepository.GetByPartitionId(transaction.PartitionKey).FirstOrDefault();

            if (existingBankTransactionStatistics != null)
            {
                existingBankTransactionStatistics.Ammount = totalAmmount;
            }
            else
            {
                existingBankTransactionStatistics = new BankTransactionStatistics(Guid.NewGuid().ToString(), transaction.PartitionKey)
                {
                    Ammount = totalAmmount
                };
            }

            bankTransactionStatisticsRepository.InsertOrUpdateEntry(existingBankTransactionStatistics);
        }
    }
}
