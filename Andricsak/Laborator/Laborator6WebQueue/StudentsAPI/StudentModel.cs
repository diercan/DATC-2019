using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class StudentModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int Year { get; set; }

        public string Faculty { get; set; }

        public string University { get; set; }

        public string CNP { get; set; }
    }
}