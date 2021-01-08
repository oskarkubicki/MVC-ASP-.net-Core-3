using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd3.DAL;
using apbd3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using apbd3.Entities;

namespace apbd3.Controllers

{

    [ApiController]
    [Route("api/students")]

    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbservice;

        List<Models.Student> lista;
        List<Models.Enrollment> lista2;

        private readonly StudentContext _StudentContext;


        public StudentsController(IDbService dbService, StudentContext student)
        
        {

            _StudentContext = student;

            _dbservice = dbService;
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

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True")) 
                using(var com =new SqlCommand())
            
            {
                com.Connection = client;
                com.CommandText = "select FirstName,LastName,BirthDate,Name,Semester from Student,Studies,Enrollment where Student.IdEnrollment=Enrollment.IdEnrollment and Enrollment.IdStudy=Studies.IdStudy";

                client.Open();
                var dr = com.ExecuteReader();
                
                lista = new List<Models.Student>();
                
                while (dr.Read())
                {

                    var st = new Models.Student();
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

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {
                com.Connection = client;

                com.CommandText = "select Enrollment.IdEnrollment,Enrollment.Semester,Enrollment.IdStudy,Enrollment.StartDate from Enrollment,Student where Student.IdEnrollment=Enrollment.IdEnrollment and Student.IndexNumber=@index";
                com.Parameters.AddWithValue("index", index);
                client.Open();
                var dr = com.ExecuteReader();
                
                lista2 = new List<Models.Enrollment>();
                
                while (dr.Read())
                {
                    var st = new Models.Enrollment();

                    st.Idenrollment = Convert.ToInt32(dr["IdEnrollment"]);
                    st.semester =  (int) dr["Semester"];
                    st.IdStudy = Convert.ToInt32(dr["IdStudy"]);
                    st.StartDate = dr["StartDate"].ToString();

                    lista2.Add(st);
                }

            }
            return Ok(lista2);
        }
        
        [HttpPost]
        
        public IActionResult createStudent(Models.Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
        
        [HttpPut("{Id}")]


        public IActionResult putStudent(int Id)
        
        {
            Models.Student student = new Models.Student();
            student.IdStudent = Id;
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok("Update complete");
        }
        
        [HttpDelete("{Id}")]


        public IActionResult deleteStudent(int Id)

        {

            return Ok("Delete complete");
        }
        
        [HttpPost("entity/change")]


        public IActionResult updateStudentE(Entities.Student student)
        {
            
            var zmiana = new Entities.Student();

            zmiana.IndexNumber = student.IndexNumber;
            
            _StudentContext.Student.Attach(zmiana);

            zmiana.IndexNumber = student.IndexNumber;
            zmiana.FirstName = student.FirstName;
            zmiana.LastName = student.LastName;
            zmiana.BirthDate = student.BirthDate;
            zmiana.IdEnrollment= student.IdEnrollment;

            _StudentContext.SaveChanges();


            return Ok(student);
        }
        
        [HttpPut("entity/add")]


        public IActionResult putStudent(Entities.Student student)
        
        {
            _StudentContext.Add<Entities.Student>(student);

            _StudentContext.SaveChanges();
            
            return Ok(student);
        }
        
        [HttpDelete("entity/del/{index}")]


        public IActionResult delStudent(string index)
        
        {
            var zmiana = new Entities.Student();

            zmiana.IndexNumber = index;

            _StudentContext.Student.Attach(zmiana);
            
            _StudentContext.Remove(zmiana);

            _StudentContext.SaveChanges();

            return Ok("deleted student");
        }

    }
}