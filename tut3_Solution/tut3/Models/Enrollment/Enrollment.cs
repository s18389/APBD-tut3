using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.Models
{

    // Tut4
    public class Enrollment
    {
        public Enrollment()
        {

        }

        public int IdEnrollment { get; set; }
        public string Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

    }

}