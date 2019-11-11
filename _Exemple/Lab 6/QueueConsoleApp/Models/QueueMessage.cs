using System.Text;

namespace QueueConsoleApp.Models
{
    public class QueueMessage
    {
        public string SenderName { get; set; }         
        public string DestinationAccount { get; set; }
        public string DestinationName { get; set; }
        public int Ammount { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"\tSenderName: {SenderName}");
            stringBuilder.AppendLine($"\tDestinationAccount: {DestinationAccount}");
            stringBuilder.AppendLine($"\tDestinationName: {DestinationName}");
            stringBuilder.AppendLine($"\tAmmount: {Ammount}");

            return stringBuilder.ToString();
        }
    }
}
