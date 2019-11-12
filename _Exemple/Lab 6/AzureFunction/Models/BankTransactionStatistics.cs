using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunction.Models
{
    class BankTransactionStatistics : TableEntity
    {
        public BankTransactionStatistics(string id, string senderName)
        {
            this.RowKey = id;
            this.PartitionKey = senderName;
        }

        public BankTransactionStatistics() { }

        public int Ammount { get; set; }
    }
}
