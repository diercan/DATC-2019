using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalDangerApi.Models;
using AnimalDangerApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnimalDangerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepo _animalsRepo;
        public AnimalController(IAnimalRepo animalsRepo)
        {
            _animalsRepo = animalsRepo;
        }
        // GET: api/Animal
        [HttpGet]
        public Task<IEnumerable<Animal>> Get()
        {
            return _animalsRepo.GetAllAnimals();
        }

        // GET: api/Animal/5
        [HttpGet("{id}", Name = "Get")]
        public Task<Animal> Get(int id)
        {
            return _animalsRepo.RetrieveSingleEntity(Convert.ToString(id));
        }

        // POST: api/Animal
        [HttpPost]
        public void Post([FromBody] Animal value)
        {
            value.PartitionKey =Convert.ToString(value.Type);
            value.RowKey = Convert.ToString(value.Id);
            if(value == null)
            {
                throw new Exception();
            }
            try
            {
               //var resultConverted = JsonConvert.DeserializeObject<Animal>(value);
                _animalsRepo.InsertOrUpdate(value);
            }
            catch(Exception)
            {
                Console.WriteLine("Not a valid response received.");
            }
        }

        // PUT: api/Animal/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
