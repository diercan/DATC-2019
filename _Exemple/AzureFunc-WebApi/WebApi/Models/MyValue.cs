using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class MyValue : TableEntity
    {
        public MyValue(string partionKey, string rowKey)
        {
            this.RowKey = rowKey;
            this.PartitionKey = partionKey;
        }

        public MyValue() 
        { 
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = Guid.NewGuid().ToString();
        }        
        public int Value { get; set; }
    }
}