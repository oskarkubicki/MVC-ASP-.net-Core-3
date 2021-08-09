using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using apbd3.DTO;
using apbd3.Entities;
using apbd3.Handlers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Enrollment = apbd3.Models.Enrollment;
using Student = apbd3.Models.Student;

namespace apbd3.Services
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        private readonly StudentContext _context;

        public SqlServerStudentDbService(StudentContext context)
        {
            _context = context;
        }

        public EnrollmentResponse EnrollStudent(Student student)
        {
            if (student.IndexNumber == null || student.Firstname == null || student.Lastname == null ||
                student.BirthDate == null || student.Studies == null) return null;

            int result1;

            var result = _context.Studies.Select(e => new
            {
                e.IdStudy,
                e.Name
            }).Where(d => d.Name.Equals(student.Studies));


            var idStudies = result.Select(e => e.IdStudy).First();
            var fromEnr = _context.Enrollment.Select(e => e).Where(e => e.Semester == 1 && e.IdStudy == idStudies);

            {
                result1 = fromEnr.Select(e => e.IdEnrollment).First();
            }

            var dont2 = _context.Student.Select(e => new {e.IndexNumber})
                .Where(d => d.IndexNumber.Equals(student.IndexNumber)).FirstOrDefault();

            if (dont2 != null) return null;

            _context.Add(new Entities.Student
            {
                FirstName = student.Firstname,
                IndexNumber = student.IndexNumber,
                LastName = student.Lastname,
                BirthDate = DateTime.Now,
                IdEnrollment = result1
            });

            _context.SaveChanges();

            var hello = _context.Enrollment.Select(e => e).Where(e =>
                e.IdStudy == idStudies && e.Semester == 1 && e.StartDate ==
                _context.Enrollment.Where(e => e.IdStudy == idStudies).Max(e => e.StartDate));


            var enrollment = new Enrollment
            {
                IdEnrollment = hello.First().IdEnrollment,
                Semester = hello.First().Semester,
                IdStudy = hello.First().IdStudy,
                StartDate = hello.First().StartDate.ToString(CultureInfo.InvariantCulture)
            };

            var er = new EnrollmentResponse(enrollment);

            return er;
        }

        public async Task<Student> GetStudentByIndexAsync(string index)
        {
            await using var client =
                new SqlConnection(
                    "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True");
            await using var com = new SqlCommand();
            com.CommandText = "Select * from student where IndexNumber=@index ";
            com.Parameters.AddWithValue("index", index);

            await using var reader = await com.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;
            var student = new Student
            {
                BirthDate = reader["Birthdate"].ToString(),
                Firstname = reader["FirstName"].ToString(),
                Lastname = reader["LastName"].ToString(),
                IdStudent = (int) reader["IdEnrollment"]
            };

            return student;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            using var client =
                new SqlConnection(
                    "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True");
            using var com = new SqlCommand();
            com.CommandText = "select * from Salt where saltID=@id";

            com.Parameters.AddWithValue("id", 1);
            client.Open();
            com.Connection = client;

            var dr = com.ExecuteReader();

            dr.Read();

            var saltc = dr["salt"].ToString();

            loginRequest.Password = PasswordGenerator.Create(loginRequest.Password, saltc);
            com.CommandText = "select * from student where Indexnumber=@index and password=@pass";
            com.Parameters.AddWithValue("pass", loginRequest.Password);
            com.Parameters.AddWithValue("index", loginRequest.Login);
            dr.Close();

            var dr2 = com.ExecuteReader();

            if (!dr2.Read()) return null;

            var response = new LoginResponse();
            response.Login = dr2["IndexNumber"].ToString();
            response.Name = dr2["LastName"].ToString();

            return response;
        }

        public PromoteResponse PromoteStudents(PromoteRequest request)
        {
            {
                //client.Open();
                //com.Connection = client;


                var result = _context.Enrollment
                    .Join(_context.Studies, p => p.IdStudy, v => v.IdStudy, (p, v) => new {p, v})
                    .Where(d => d.p.Semester == request.Semester && d.v.Name.Equals(request.Studies)).FirstOrDefault();


                if (result == null) return null;

                var name = new SqlParameter("@name", request.Studies);
                var semester = new SqlParameter("@semester", request.Semester);

                var list1 = _context.Database.ExecuteSqlCommand("exec Promotion @name, @semester", name, semester);


                var result3 = _context.Enrollment
                    .Join(_context.Studies, p => p.IdStudy, v => v.IdStudy, (p, v) => new {p, v}).Where(d =>
                        d.p.Semester == request.Semester + 1 && d.v.Name.Equals(request.Studies)).FirstOrDefault();

                var enrollment = new Enrollment();

                enrollment.IdStudy = result3.p.IdStudy;
                enrollment.Semester = result3.p.Semester;
                var startDate = result3.p.StartDate;
                enrollment.StartDate = startDate.ToString();

                var promotion = new PromoteResponse(enrollment);

                return promotion;
            }
        }

        public void SaveLogData(string data)
        {
            try
            {
                using var w =
                    File.AppendText("C:\\Users\\virion\\Desktop\\apbd3\\apbd3\\apbd3\\apbd3\\Log.txt");
                w.Write(data);

                w.Flush();
                w.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SaveToken(string login, string name, string token)
        {
            using var client =
                new SqlConnection(
                    "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True");
            using var com = new SqlCommand();
            client.Open();
            com.Connection = client;
            com.CommandText = "insert into RefreshToken (Login,Name,RefreshToken) values (@login,@name,@token)";
            com.Parameters.AddWithValue("login", login);
            com.Parameters.AddWithValue("name", name);
            com.Parameters.AddWithValue("token", token);

            com.ExecuteNonQuery();
        }

        public TokenResponse CheckToken(string token)
        {
            using (var client =
                new SqlConnection(
                    "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {
                client.Open();
                com.Connection = client;

                com.CommandText = "select * from RefreshToken where RefreshToken.RefreshToken=@token ";
                com.Parameters.AddWithValue("token", token);

                var dr = com.ExecuteReader();

                if (!dr.Read()) return null;

                var response = new TokenResponse();
                response.Login = dr["Login"].ToString();
                response.Name = dr["Name"].ToString();

                return response;
            }
        }
    }
}