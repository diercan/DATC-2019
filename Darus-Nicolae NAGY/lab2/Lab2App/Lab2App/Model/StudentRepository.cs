using System.Collections;
using System.Collections.Generic;
using Lab2App.DTO;

namespace Lab2App.Model
{
    public static class StudentRepository
    {
        private static readonly IDictionary<long, Student> Students = new Dictionary<long, Student>();
        private static long _lastIndex = 0;

        public static Student GetById(long index) => Students[index];

        public static void AddStudent(NewStudentDto studentDto)
        {
            _lastIndex++;
            Student student = new Student(_lastIndex, studentDto.Name, studentDto.Faculty, studentDto.Year);
            Students[student.Id] = student;
        }

        public static IEnumerable<Student> All => Students.Values;

        public static void Delete(in long id) => Students.Remove(id);
    }
}