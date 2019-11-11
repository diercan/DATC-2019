using System.Collections.Generic;
using System.Linq;
using DATC_lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATC_lab2.Controllers
{
    [Route("api/[controller]")]
    // This controller does not return ActionResult objects. This limits us in the possible return values.
    public class StudentController : Controller
    {
        private static List<Student> _studentList = new List<Student>();

        public StudentController()
        {
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _studentList;
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return _studentList.ElementAt(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Student student)
        {
            _studentList.Add(student);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Student student)
        {
            if(_studentList.Count > id)
            {
                _studentList[id] = student;
            }            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if(_studentList.Count > id)
            {
                _studentList.Remove(_studentList.ElementAt(id));
            }
        }
    }
}
