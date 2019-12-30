using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace StudentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
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
            // Must be implemented
            return null;
        }

        // POST api/students
        [HttpPost]
        public async Task Post([FromBody] StudentModel student)
        {
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

            using (var service = new StudentsService())
            {
                await service.Initialize();
                await service.AddStudent(studentEntity);
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
