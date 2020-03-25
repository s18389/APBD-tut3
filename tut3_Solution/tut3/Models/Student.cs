using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.Models
{
    public class Student
    {
        public Student(int IdStudent, string FirstName, string LastName, string IndexNumber)
        {
            this.IdStudent = IdStudent;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.IndexNumber = IndexNumber;
        }

        public Student()
        {

        }

        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
    }
}
