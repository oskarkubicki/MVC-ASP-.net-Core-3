using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using apbd3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd3.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {


        [HttpPost]
        public IActionResult AddStudent(Student student)
        {

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())


            {
                if (student.IndexNumber == null || student.Firstname == null || student.Lastname == null || student.BirthDate == null || student.Studies == null)
                {

                    return BadRequest("error missing information");
                }
                else

                {

                    int idEnrollment;
                    client.Open();
                    com.Connection = client;
                    var tran = client.BeginTransaction();

                    com.Transaction = tran;


                    com.CommandText = "select * from Studies where Name=@index";
                    com.Parameters.AddWithValue("index", student.Studies);
                   
                    var dr = com.ExecuteReader();


                    if (!dr.Read())
                    {
                        tran.Rollback();

                    }


                    int idStudies = (int)dr["IdStudy"];


                    dr.Close();


                    com.CommandText = "SELECT * FROM Enrollment WHERE Semester=1 AND IdStudy=@IdStud";

                    com.Parameters.AddWithValue("IdStud", idStudies);


                    var dr2 = com.ExecuteReader();



                    if (!dr2.Read())
                    {

                        dr2.Close();
                        com.CommandText = "Select max(idenrollment) from enrollment";

                        var dr3 = com.ExecuteReader();

                        dr3.Read();

                         idEnrollment = dr3.GetInt32(0);

                        com.CommandText = "INSERT INTO Enrollment(idEnrollment, semester, idStudy,StartDate) VALUES (@idE, @Semester,@IdStudy,@sd";


                        com.Parameters.AddWithValue("idE", idEnrollment + 1);
                        com.Parameters.AddWithValue("Semester", 1);
                        com.Parameters.AddWithValue("IdStudy", idStudies);
                        com.Parameters.AddWithValue("sd", DateTime.Now.ToString());


                        dr3.Close();

                    }
                    else
                    {

                        idEnrollment = (int) dr2["IdEnrollment"];

                    }
               
                  


                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName,Birthdate,IdEnrollment) VALUES (@Indexn,@FirstName, @LastName,@Birthdate,@Studies)";
                    //...
                    com.Parameters.AddWithValue("FirstName", student.Firstname);
                    com.Parameters.AddWithValue("Indexn", student.IndexNumber);
                    com.Parameters.AddWithValue("LastName", student.Lastname);
                    com.Parameters.AddWithValue("Birthdate", DateTime.Parse(student.BirthDate));
                    com.Parameters.AddWithValue("Studies",idEnrollment );


                    //...
                    com.ExecuteNonQuery();

                    tran.Commit(); //make all the changes in db visible to another users


                    com.CommandText = "Select * from enrollment where idstudy=@idstudies and semester=1 and max(startdate)";

                    var dr4 = com.ExecuteReader();


                    var enrollment = new Enrollment();
                    while (dr4.Read())
                    {
                        enrollment.Idenrollment = (int)dr4["idEnrollment"];
                        enrollment.semester = (string)dr4["Semester"];
                        enrollment.IdStudy = (int)dr4["IdStudy"];
                        enrollment.StartDate = (string)dr4["idEnrollment"];

                    }

                    return new ObjectResult(enrollment) { StatusCode = StatusCodes.Status201Created };
                }


            }
        }
    }
}
