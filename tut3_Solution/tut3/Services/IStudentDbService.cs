using tut3.DTOs.Requests;

namespace tut3.Services
{
    public interface IStudentServiceDb
    {
        void EnrollStudent(EnrollStudentRequest req);
        void PromoteStudents(StudentPromoteRequest req);
    }
}
