using AnimalDangerApi.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDangerApi.Repositories
{
    public interface IAnimalRepo
    {
        public Task<TableResult> InsertOrMerge(Animal animal);
        public Task<Animal> RetrieveSingleEntity(string id);
        public Task<IEnumerable<Animal>> GetAllAnimals();
    }
}
