using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDangerApi.Models
{
    public class Animal : TableEntity
    {
        public int Id { get; set; }
        public double[] Location { get; set; }
        public string Name { get; set; }
        public AnimalType Type { get; set; }
    }
}
