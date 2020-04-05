using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using tut3.DTOs.Requests;
using tut3.DTOs.Responses;


namespace tut3.Services
{
    public class SqlServerStudentDbService : ControllerBase , IStudentServiceDb
    {
        public void EnrollStudent(EnrollStudentRequest request)
        {
            using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s18389;Integrated Security=True; MultipleActiveResultSets=True;"))
            {
                using (var command = new SqlCommand())
                {
                    command.CommandText = "SELECT * FROM Studies WHERE Name = @Name";
                    command.Parameters.AddWithValue("Name", request.Studies);
                    command.Connection = connection;

                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    command.Transaction = transaction;

                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        //Studies not exitsts 404
                        transaction.Rollback();
                        BadRequest("The type of this studies does not exist!");
                    }


                    int idStudies = Int32.Parse(reader["IdStudy"].ToString());
                    reader.Close();
                    command.CommandText = "SELECT * FROM Enrollment WHERE Semester=1 AND IdStudy=@IdStudy";
                    command.Parameters.AddWithValue("IdStudy", idStudies);
                    var reader2 = command.ExecuteReader();
                    int IdEnrollment = 0;
                    if (!reader2.Read())
                    {
                        reader2.Close();
                        //Enrollment with semester = 1 or id studies dont exist in enrollment, make a new one
                        command.CommandText = "SELECT * FROM Enrollment WHERE IdEnrollment = (SELECT MAX(IdEnrollment) FROM Enrollment)";
                        SqlDataReader reader3 = command.ExecuteReader();
                        if (reader3.Read())
                        {
                            IdEnrollment = Int32.Parse(reader3["IdEnrollment"].ToString());
                            IdEnrollment++;
                        }
                        reader3.Close();
                        int newSemester = 1;
                        DateTime newStartDate = DateTime.Now;

                        command.CommandText = "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (@IdEnrollment, @newSemester, @newIdStudy, @newStartDate)";
                        command.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                        command.Parameters.AddWithValue("newSemester", idStudies);
                        command.Parameters.AddWithValue("newIdStudy", newSemester);
                        command.Parameters.AddWithValue("newStartDate", newStartDate);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        //such an enrollment exits
                        IdEnrollment = Int32.Parse(reader2["IdEnrollment"].ToString());
                        reader2.Close();
                    }

                    //Creating a new student
                    //cheking if index already is in database
                    command.CommandText = "SELECT * FROM Student WHERE IndexNumber = @indexNumberFromReuest";
                    command.Parameters.AddWithValue("indexNumberFromReuest", request.IndexNumber);
                    var reader4 = command.ExecuteReader();
                    if (reader4.Read())
                    {
                        //index already exists
                        transaction.Rollback();
                        BadRequest("A student with this index number is already exist!");
                    }
                    else
                    {
                        reader4.Close();
                        //index not exists, creating a new record of student table
                        command.CommandText = "INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @studentIdEnrollment)";
                        command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                        command.Parameters.AddWithValue("FirstName", request.FirstName);
                        command.Parameters.AddWithValue("LastName", request.LastName);
                        command.Parameters.AddWithValue("BirthDate", request.BirthDate);
                        command.Parameters.AddWithValue("studentIdEnrollment", IdEnrollment);
                        command.ExecuteNonQuery();
                    }
                    Ok();

                }
            }

        }

        public void PromoteStudents(StudentPromoteRequest request)
        {
            int IdEnrollmentResponse = 0;
            string SemesterResponse = null;
            int IdStudyResponse = 0;

            using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s18389;Integrated Security=True; MultipleActiveResultSets=True;"))
            {

                using (var command = new SqlCommand())
                {

                    command.CommandText = "SELECT Enrollment.IdEnrollment, Enrollment.Semester, Enrollment.IdStudy, Enrollment.StartDate FROM Enrollment, Studies WHERE Enrollment.IdStudy = Studies.IdStudy AND Studies.Name = @nameStudies AND Enrollment.Semester = @numberOfSemester;";
                    command.Parameters.AddWithValue("nameStudies", request.Studies);
                    command.Parameters.AddWithValue("numberOfSemester", request.Semester);
                    command.Connection = connection;
                    connection.Open();
                    var transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        //Studies not exitsts 404
                        transaction.Rollback();
                        BadRequest("The type of this studies or semester wrong!");
                    }
                    else
                    {
                        command.CommandText = "EXECUTE PromoteStudents(@nameStudies, @numberOfSemester)";
                        command.Parameters.AddWithValue("nameStudies", request.Studies);
                        command.Parameters.AddWithValue("numberOfSemester", request.Semester);
                    }

                }
            }
            var response = new EnrollPromoteResponse();
            response.IdEnrollment = IdEnrollmentResponse;
            response.Semster = SemesterResponse;
            response.IdStudy = IdStudyResponse;
            Ok(response);
        }
    }
}
