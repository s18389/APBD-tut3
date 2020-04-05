using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.DTOs.Responses
{
    public class EnrollPromoteResponse
    {
        public int IdEnrollment { set; get; } 
        public string Semster { set; get; } 
        public int IdStudy { set; get; } 
        public DateTime StartDate { set; get; } 
    }
}
