using System.Collections.Generic;
using System.Linq;
using DATC_lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATC_lab2.Controllers
{
    [Route("api/[controller]")]
    // This controller returns ActionResult objects. This allows us more control over the returned status code: NotFound, Created, etc
    public class Student2Controller : Controller
    {
        private static List<Student> _studentList = new List<Student>();

        public Student2Controller()
        {
        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_studentList);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var student = _studentList.Where(s => s.Id == id).FirstOrDefault();

            if(student != null)
            {
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody]Student student)
        {
            if(_studentList.Where(s => s.Id == student.Id).FirstOrDefault() != null)
            {
                return BadRequest($"Id {student.Id} is already used for a different Student");
            }
            else
            {
                _studentList.Add(student);
                return Created(student.Id.ToString(), student);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]Student student)
        {
            if(student.Id == 0)
            {
                student.Id = id;
            }

            var existingStudent = _studentList.Where(s => s.Id == student.Id).FirstOrDefault();
            
            if(existingStudent != null)
            {
                var index = _studentList.IndexOf(existingStudent);
                _studentList[index] = student;
                return Accepted(student); 
            }
            else
            {
                _studentList.Add(student);
                return Created(student.Id.ToString(), student);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingStudent = _studentList.Where(s => s.Id == id).FirstOrDefault();

            if(existingStudent != null)
            {
                _studentList.Remove(existingStudent);
                return Accepted();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
