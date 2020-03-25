using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut3.DAL;
using tut3.Models;

namespace tut3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IServiceDB _serviceDB;

        public StudentController(IServiceDB serviceDB)
        {
            _serviceDB = serviceDB;
        }
        
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if(id == 1)
            {
                return Ok("Michalski");
            }else if (id == 2)
            {
                return Ok("Nasirov");
            }
            return NotFound("Student not found!");
        }

        /*
        [HttpGet]
        public string GetStudents(string orderBy)
        { 
            return $"Michalski, Nasirov, Sim sorting={orderBy}";
           
        }
        */


        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 30000)}";
            return Ok(student);

        }

        [HttpDelete("{id}")]
        public IActionResult deleteStudent(int id)
        {
            return Ok($"Delete {id}");
        }

        [HttpPut("{id}")]
        public IActionResult updateStudent(int id)
        {
            return Ok($"Update {id}");
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_serviceDB.GetStudents());
        }

    }
}