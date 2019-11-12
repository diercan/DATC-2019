using System;
using System.Linq;
using AzureFunction.Models;
using AzureFunction.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunction
{
    public static class TableStorageAzureFunction
    {
        public static string ConnectionString = "";
        public static string My_Name = "AlexandruRus";

        //[FunctionName("TimerTriggerCSharp")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var bankTransactionRepository = new TableStorageService<BankTransaction>(log, ConnectionString, $"{My_Name}BankTransaction");
            var bankTransactionStatisticsRepository = new TableStorageService<BankTransactionStatistics>(log, ConnectionString, $"{My_Name}BankTransactionStatistics");

            SeedData(bankTransactionRepository);

            // Generate BankTransactionStatistics from all senders by aggregating the total sum of their transactions

            var entities = bankTransactionRepository.GetAllEntries();
            var senders = entities.Select(e => e.PartitionKey).Distinct();

            foreach(var sender in senders)
            {
                var transactions = bankTransactionRepository.GetByPartitionId(sender);
                var totalAmmount = transactions.Sum(t => t.Ammount);

                var existingBankTransactionStatistics = bankTransactionStatisticsRepository.GetByPartitionId(sender).FirstOrDefault();

                if(existingBankTransactionStatistics != null)
                {
                    existingBankTransactionStatistics.Ammount = totalAmmount;
                }
                else
                {
                    existingBankTransactionStatistics = new BankTransactionStatistics(Guid.NewGuid().ToString(), sender)
                    {
                        Ammount = totalAmmount
                    };
                }

                bankTransactionStatisticsRepository.InsertOrUpdateEntry(existingBankTransactionStatistics);
            }
        }

        private static void SeedData(TableStorageService<BankTransaction> tableStorageService)
        {
            var transactions = new BankTransaction[]
            {
                new BankTransaction("1", "John Smith")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Amanda"
                },
                new BankTransaction("2", "John Smith")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Elton"
                },
                new BankTransaction("3", "Elton James")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Amanda"
                },
                new BankTransaction("4", "John Smith")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Elton"
                },
                new BankTransaction("5", "Elton James")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Amanda"
                },
                new BankTransaction("6", "Amanda Jefferson")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "John"
                },
                new BankTransaction("7", "Amanda Jefferson")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Elton"
                },
                new BankTransaction("8", "John Smith")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Elton"
                },
                new BankTransaction("9", "Amanda Jefferson")
                {
                    Ammount = new Random().Next(1, 100),
                    DestinationAccount = Guid.NewGuid().ToString(),
                    DestinationName = "Elton"
                },
            };

            foreach(var transaction in transactions)
            {
                tableStorageService.InsertOrUpdateEntry(transaction);
            }
        }
    }
}
