using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Text;
using System.Threading;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace StudentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        const string ServiceBusConnectionString = "";
        const string QueueName = "queue-3";
        static IQueueClient queueClient;
        // GET api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentEntity>>> Get()
        {
            using (var service = new StudentsService())
            {
                await service.Initialize();
                return await service.GetStudents();
            }
        }

        // GET api/students/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return null;
        }

        // POST api/students
        [HttpPost]
        public async Task Post([FromBody] StudentModel student)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            if (string.IsNullOrEmpty(student.University) || string.IsNullOrEmpty(student.CNP))
            {
                throw new Exception("The student does not have university or CNP set!");
            }

            var studentEntity = new StudentEntity(student.University, student.CNP)
            {
                Email = student.Email,
                Faculty = student.Faculty,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhoneNumber = student.PhoneNumber,
                Year = student.Year
            };

            await SendMessagesAsync(studentEntity);
            await queueClient.CloseAsync();
        }

        static async Task SendMessagesAsync(StudentEntity student)
        {
            try{ 
                string studentSerialized = JsonConvert.SerializeObject(student);

                var studentEncoded = new Message(Encoding.UTF8.GetBytes(studentSerialized));
                Console.WriteLine($"Sending student: {studentEncoded}");

                await queueClient.SendAsync(studentEncoded);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {ex.Message}");
            }
        }


        // PUT api/students/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // Must be implemented
        }

        // DELETE api/students/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Must be implemented
        }
    }
}
