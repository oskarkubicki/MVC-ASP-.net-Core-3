using apbd3.DTO;
using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.Services
{
    public interface IStudentsDbService
    {
        public EnrollmentResponse EnrollStudent(Student student);

        public PromoteResponse PromoteStudents(PromoteRequest request);

        public Task<Student> GetStudentByIndexAsync(string index);


        public void SaveLogData(string data);


        public LoginResponse Login(LoginRequest loginRequest);
    
    }
}
