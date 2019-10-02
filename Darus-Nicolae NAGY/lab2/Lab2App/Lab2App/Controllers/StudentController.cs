using System.Collections.Generic;
using Lab2App.DTO;
using Lab2App.Model;
using Microsoft.AspNetCore.Mvc;

namespace Lab2App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Student> GetAllStudents()
        {
            return StudentRepository.All;
        }

        [HttpGet]
        [Route("{id}")]
        public Student GetById(long id)
        {
            return StudentRepository.GetById(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public void DeleteById(long id)
        {
            StudentRepository.Delete(id);
        }

        [HttpPost]
        public void AddStudent([FromBody] NewStudentDto studentDto)
        {
            StudentRepository.AddStudent(studentDto);
        }
    }
}