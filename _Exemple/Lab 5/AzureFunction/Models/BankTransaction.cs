using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunction.Models
{
    public class BankTransaction : TableEntity
    {
        public BankTransaction(string id, string senderName)
        {
            this.RowKey = id;
            this.PartitionKey = senderName;
        }

        public BankTransaction() { }

        public string DestinationAccount { get; set; }
        public string DestinationName { get; set; }
        public int Ammount { get; set; }
    }
}