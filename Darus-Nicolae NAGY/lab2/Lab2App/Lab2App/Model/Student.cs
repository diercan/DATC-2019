namespace Lab2App.Model
{
    public class Student
    {
        public Student(long id, string name, string faculty, int year)
        {
            Id = id;
            Name = name;
            Faculty = faculty;
            Year = year;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public int Year { get; set; }
    }
}