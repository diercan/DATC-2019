using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Andricsak
{
    public class StudentEntity : TableEntity 
    {
        public StudentEntity(string university,string id)
        {
            this.PartitionKey = university;
            this.RowKey = id;
        }
        public StudentEntity(){}
        public string University{get;set;}

        public string ID{get;set;}
        public string Name{get;set;}
    }
}