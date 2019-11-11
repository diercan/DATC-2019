namespace AzureFunction.Models
{
    public class QueueMessage
    {
        public string SenderName { get; set; }         
        public string DestinationAccount { get; set; }
        public string DestinationName { get; set; }
        public int Ammount { get; set; }
    }
}
