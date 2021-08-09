using System;
using System.Collections.Generic;
using System.Linq;
using apbd3.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Enrollment = apbd3.Models.Enrollment;
using Student = apbd3.Models.Student;

namespace apbd3.Controllers

{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _studentContext;
        private List<Enrollment> _enrollments;

        private List<Student> _students;

        public StudentsController(StudentContext student, IConfiguration configuration)
        {
            _studentContext = student;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        [HttpGet("entity")]
        public IActionResult GetStudentsE()

        {
            var students = _studentContext.Student.ToList();
            return Ok(students);
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            using (var client =
                new SqlConnection(Configuration.GetConnectionString("DefaultConnectionString")
                ))
            using (var com = new SqlCommand())

            {
                com.Connection = client;
                com.CommandText =
                    @"select FirstName,
                            LastName,
                            BirthDate,
                            Name,
                            Semester
                    from Student,Studies,Enrollment
                    where Student.IdEnrollment=Enrollment.IdEnrollment and Enrollment.IdStudy=Studies.IdStudy";

                client.Open();
                var dr = com.ExecuteReader();

                _students = new List<Student>();

                while (dr.Read())
                {
                    var st = new Student();
                    st.Firstname = dr["FirstName"].ToString();
                    st.Lastname = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Studies = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();

                    _students.Add(st);
                }
            }

            return Ok(_students);
        }

        [HttpGet("secret/{index}")]
        public IActionResult GetStudent(string index)
        {
            using (var client =
                new SqlConnection(
                    Configuration.GetConnectionString("DefaultConnectionString")))
            using (var com = new SqlCommand())

            {
                com.Connection = client;
                com.CommandText =
                    @"select Enrollment.IdEnrollment,
                    Enrollment.Semester,
                    Enrollment.IdStudy,
                    Enrollment.StartDate
                     from Enrollment,Student 
                     where Student.IdEnrollment=Enrollment.IdEnrollment 
                     and Student.IndexNumber=@index";
                com.Parameters.AddWithValue("index", index);
                client.Open();
                var dr = com.ExecuteReader();

                _enrollments = new List<Enrollment>();

                while (dr.Read())
                {
                    var st = new Enrollment
                    {
                        IdEnrollment = Convert.ToInt32(dr["IdEnrollment"]),
                        Semester = (int) dr["Semester"],
                        IdStudy = Convert.ToInt32(dr["IdStudy"]),
                        StartDate = dr["StartDate"].ToString()
                    };
                    _enrollments.Add(st);
                }
            }

            return Ok(_enrollments);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{Id}")]
        public IActionResult PutStudent(int Id)

        {
            var student = new Student();
            student.IdStudent = Id;
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok("Update complete");
        }

        [HttpPost("entity/change")]
        public IActionResult UpdateStudentE(Entities.Student student)
        {
            var zmiana = new Entities.Student();

            zmiana.IndexNumber = student.IndexNumber;

            _studentContext.Student.Attach(zmiana);

            zmiana.IndexNumber = student.IndexNumber;
            zmiana.FirstName = student.FirstName;
            zmiana.LastName = student.LastName;
            zmiana.BirthDate = student.BirthDate;
            zmiana.IdEnrollment = student.IdEnrollment;

            _studentContext.SaveChanges();

            return Ok(student);
        }

        [HttpPut("entity/add")]
        public IActionResult PutStudent(Entities.Student student)

        {
            _studentContext.Add(student);
            _studentContext.SaveChanges();

            return Ok(student);
        }

        [HttpDelete("entity/del/{index}")]
        public IActionResult DelStudent(string index)

        {
            var zmiana = new Entities.Student
            {
                IndexNumber = index
            };
            _studentContext.Student.Attach(zmiana);
            _studentContext.Remove(zmiana);
            _studentContext.SaveChanges();

            return Ok("deleted student");
        }
    }
}