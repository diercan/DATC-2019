using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureFunction.Models
{
    public class BankTransaction : TableEntity
    {
        public BankTransaction(string id, string senderName)
        {
            this.RowKey = id;
            this.PartitionKey = senderName;
        }

        public BankTransaction(QueueMessage queueMessage)
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = queueMessage.SenderName;
            this.DestinationAccount = queueMessage.DestinationAccount;
            this.DestinationName = queueMessage.DestinationName;
            this.Ammount = queueMessage.Ammount;
        }

        public BankTransaction() { }

        public string DestinationAccount { get; set; }
        public string DestinationName { get; set; }
        public int Ammount { get; set; }        
    }
}