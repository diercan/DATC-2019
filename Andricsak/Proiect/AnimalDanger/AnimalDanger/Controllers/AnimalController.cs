using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalDangerApi.Models;
using AnimalDangerApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AnimalDangerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepo _animalsRepo;
        private readonly AlertRepo _alertsRepo;
        public AnimalController(IAnimalRepo animalsRepo,AlertRepo alertsRepo)
        {
            _animalsRepo = animalsRepo;
            _alertsRepo = alertsRepo;
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
                return;
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

        // POST: api/Animal
        [HttpPost]
        [Route("PostAlert")]
        public async Task PostAlert([FromBody] Alert value)
        {
            if (value == null)
                return;
            else
            {
                value.PartitionKey = value.City;
                value.RowKey = Convert.ToString(value.Id);

                await _alertsRepo.InsertOrUpdate(value);       
            }
        }
        // GET: api/Animal/Alerts
        [HttpGet]
        [Route("Alerts")]
        public Task<IEnumerable<Alert>> GetAlerts()
        {
            return _alertsRepo.GetAllAlerts();
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
