using System;
using System.Collections.Generic;
using System.Linq;
using apbd3.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Enrollment = apbd3.Models.Enrollment;
using Student = apbd3.Models.Student;

namespace apbd3.Controllers

{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _StudentContext;

        private List<Student> lista;
        private List<Enrollment> lista2;

        public StudentsController(StudentContext student)

        {
            _StudentContext = student;
        }

        [HttpGet("entity")]
        public IActionResult GetStudentsE()

        {
            var students = _StudentContext.Student.ToList();
            return Ok(students);
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            using (var client =
                new SqlConnection(
                    "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {
                com.Connection = client;
                com.CommandText =
                    @"    select    FirstName,
                                LastName,
                                BirthDate,
                                Name,
                                Semester
                        from 
                         Student,
                         Studies,
                         Enrollment
                        where 
                          Student.IdEnrollment=Enrollment.IdEnrollment and Enrollment.IdStudy=Studies.IdStudy";

                client.Open();
                var dr = com.ExecuteReader();

                lista = new List<Student>();

                while (dr.Read())
                {
                    var st = new Student();
                    st.Firstname = dr["FirstName"].ToString();
                    st.Lastname = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Studies = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();

                    lista.Add(st);
                }
            }

            return Ok(lista);
        }

        [HttpGet("secret/{index}")]
        public IActionResult GetStudent(string index)
        {
            using (var client =
                new SqlConnection(
                    "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {
                com.Connection = client;

                com.CommandText =
                    "select Enrollment.IdEnrollment,Enrollment.Semester,Enrollment.IdStudy,Enrollment.StartDate from Enrollment,Student where Student.IdEnrollment=Enrollment.IdEnrollment and Student.IndexNumber=@index";
                com.Parameters.AddWithValue("index", index);
                client.Open();
                var dr = com.ExecuteReader();

                lista2 = new List<Enrollment>();

                while (dr.Read())
                {
                    var st = new Enrollment
                    {
                        IdEnrollment = Convert.ToInt32(dr["IdEnrollment"]),
                        Semester = (int) dr["Semester"],
                        IdStudy = Convert.ToInt32(dr["IdStudy"]),
                        StartDate = dr["StartDate"].ToString()
                    };

                    lista2.Add(st);
                }
            }

            return Ok(lista2);
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

        [HttpDelete("{Id}")]
        public IActionResult DeleteStudent(int Id)

        {
            return Ok("Delete complete");
        }

        [HttpPost("entity/change")]
        public IActionResult UpdateStudentE(Entities.Student student)
        {
            var zmiana = new Entities.Student();

            zmiana.IndexNumber = student.IndexNumber;

            _StudentContext.Student.Attach(zmiana);

            zmiana.IndexNumber = student.IndexNumber;
            zmiana.FirstName = student.FirstName;
            zmiana.LastName = student.LastName;
            zmiana.BirthDate = student.BirthDate;
            zmiana.IdEnrollment = student.IdEnrollment;

            _StudentContext.SaveChanges();

            return Ok(student);
        }

        [HttpPut("entity/add")]
        public IActionResult PutStudent(Entities.Student student)

        {
            _StudentContext.Add(student);

            _StudentContext.SaveChanges();

            return Ok(student);
        }

        [HttpDelete("entity/del/{index}")]
        public IActionResult DelStudent(string index)

        {
            var zmiana = new Entities.Student
            {
                IndexNumber = index
            };
            _StudentContext.Student.Attach(zmiana);
            _StudentContext.Remove(zmiana);
            _StudentContext.SaveChanges();

            return Ok("deleted student");
        }
    }
}