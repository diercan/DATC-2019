using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using WebApi.Storage;

namespace DATC_lab2.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private TableStorageService<MyValue> valuesRepository;
        private const string TableStorageConnectionString = "";
        private const string ServiceBusConnectionString = "";
        private const string TableName = "MySumTable";
        private const string QueueName = "myqueue-1";
        private ILogger _logger;        

        public ValuesController(ILogger<ValuesController> logger)
        {
            this._logger = logger;
            valuesRepository = new TableStorageService<MyValue>(logger, TableStorageConnectionString, TableName);
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<MyValue> Get()
        {
            return valuesRepository.GetAllEntries();
        }

        [HttpGet("latest")]
        public MyValue GetLatestValue()
        {
            // this is very inefficient
            return valuesRepository.GetAllEntries().OrderBy(v => v.Timestamp).Last();
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]MyValue value)
        {
            var queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            var queueMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
            _logger.LogInformation($"Sending message:\n{value.ToString()}");
            await queueClient.SendAsync(queueMessage);
        }
    }
}
