using System;
using System.Collections.Generic;
using StudentNameSpace;
namespace StudentiNameSpace
{
    public static  class Studenti{
        public static List<Student> students = new List<Student>()
        {
            new Student(1,"Vasile","Politehnica",3),
            new Student(2,"Ion","Politehnica",1),
            new Student(3,"Marie","Politehnica",2)
        };
    }
}