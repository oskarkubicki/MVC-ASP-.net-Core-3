using System.Threading.Tasks;
using apbd3.DTO;
using apbd3.Models;

namespace apbd3.Services
{
    public interface IStudentsDbService
    {
        public EnrollmentResponse EnrollStudent(Student student);
        public PromoteResponse PromoteStudents(PromoteRequest request);
        public Task<Student> GetStudentByIndexAsync(string index);
        public void SaveLogData(string data);
        public LoginResponse Login(LoginRequest loginRequest);
        public void SaveToken(string login, string name, string token);
        public TokenResponse CheckToken(string token);
    }
}