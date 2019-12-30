using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundWorker.Models
{
    public class Alert : TableEntity
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public string City { get; set; }
    }
}
