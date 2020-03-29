using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        /*
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


        //task 3.2 From tut4
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var listOfStudents = new HashSet<Student>();
            using (var sqlConnection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s18389;Integrated Security=True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT * FROM Student;";

                    sqlConnection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var student = new Student();
                        student.IndexNumber = reader["IndexNumber"].ToString();
                        student.FirstName = reader["FirstName"].ToString();
                        student.LastName = reader["LastName"].ToString();
                        student.BirthDate = DateTime.Parse(reader["BirthDate"].ToString());
                        student.IdEnrollment = Int32.Parse(reader["IdEnrollment"].ToString());
                        listOfStudents.Add(student);
                    }

                }
            }

            return Ok(listOfStudents);
        }

        //task 3.3, 3.5 From tut4
        [HttpGet("{index}")]
        public IActionResult GetStudentSemesters(string index)
        {
            var listOfStudentEnrollment = new HashSet<Enrollment>();
            using (var sqlConnection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s18389;Integrated Security=True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = "SELECT * FROM Enrollment, Student WHERE Enrollment.IdEnrollment = Student.IdEnrollment AND Student.IndexNumber = @index;";
                    command.Parameters.AddWithValue("index", index);

                    sqlConnection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var enrollment = new Enrollment();
                        enrollment.IdEnrollment = Int32.Parse(reader["IdEnrollment"].ToString());
                        enrollment.Semester = reader["Semester"].ToString();
                        enrollment.IdStudy = Int32.Parse(reader["IdStudy"].ToString());
                        enrollment.StartDate = DateTime.Parse(reader["StartDate"].ToString());
                        listOfStudentEnrollment.Add(enrollment);
                    }

                }
            }

            return Ok(listOfStudentEnrollment);

        }
    }
}