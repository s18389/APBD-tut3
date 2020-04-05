using Microsoft.AspNetCore.Mvc;
using tut3.DTOs.Requests;
using tut3.DTOs.Responses;
using tut3.Services;

namespace tut3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private IStudentServiceDb _service;

        public EnrollmentController(IStudentServiceDb service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);
            var response = new EnrollStudentResponse();
            response.Semester = 1;
            return Ok(response);
        }


        [HttpPost("promotions")]
        public IActionResult PromoteStudents(StudentPromoteRequest request)
        {
            _service.PromoteStudents(request);
            return Ok();
        }

    }
}