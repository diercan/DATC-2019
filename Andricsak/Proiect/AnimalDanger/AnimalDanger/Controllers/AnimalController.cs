using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalDangerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimalDangerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        // GET: api/Animal
        [HttpGet]
        public IEnumerable<Animal> Get()
        {
            IEnumerable<Animal> animals = new List<Animal>()
            {
                 new Animal { Id = 1, Name = "Mazarica" },
                 new Animal { Id = 2, Name = "Gigel" }
            };
            return animals;
        }

        // GET: api/Animal/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Animal
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
