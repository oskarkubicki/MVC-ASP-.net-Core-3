using apbd3.DTO;
using apbd3.Entities;
using apbd3.Handlers;
using apbd3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace apbd3.Services
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        private readonly StudentContext _context;

        public SqlServerStudentDbService(StudentContext context)
        {

            _context = context;

        }
        public EnrollmentResponse EnrollStudent(Models.Student student)
        {
        


            
                if (student.IndexNumber == null || student.Firstname == null || student.Lastname == null || student.BirthDate == null || student.Studies == null)
                {

                    return null;
                }
                else
                {
                    int result1;
                    //int idEnrollment;
                    //client.Open();
                    //com.Connection = client;
                    //var tran = client.BeginTransaction();
                    //com.Transaction = tran;
                    //com.CommandText = "select * from Studies where Name=@index";
                    //com.Parameters.AddWithValue("index", student.Studies);





                    var result = _context.Studies.Select(e => new {


                        e.IdStudy,
                        e.Name
                    }).Where(d => d.Name.Equals(student.Studies));


                    //var dr = com.ExecuteReader();

                    if (result == null)
                    {
                        return null;
                    }



                    int idStudies = result.Select(e => e.IdStudy).First();


                    //dr.Close();

                    var fromEnr= _context.Enrollment.Select(e =>e ).Where(e => e.Semester==1 && e.IdStudy==idStudies);


                    //com.CommandText = "SELECT * FROM Enrollment WHERE Semester=1 AND IdStudy=@IdStud";

                    //com.Parameters.AddWithValue("IdStud", idStudies);

                    //var dr2 = com.ExecuteReader();

                    if (fromEnr==null)
                    {

                        //dr2.Close();
                        //com.CommandText = "Select max(idenrollment) from enrollment";

                         result1 = _context.Enrollment.Max(e => e.IdEnrollment);

                        //var dr3 = com.ExecuteReader();

                        //dr3.Read();

                        //idEnrollment = dr3.GetInt32(0);

                        //dr3.Close();

                        var newEnroll = new Entities.Enrollment()
                        {
                            IdEnrollment = result1 + 1,
                            Semester = 1,
                            IdStudy = idStudies,
                            StartDate = DateTime.Now


                        };

                        _context.Enrollment.Add(newEnroll);

                        _context.SaveChanges();


                        //com.CommandText = "INSERT INTO Enrollment(idEnrollment, semester, idStudy,StartDate) VALUES (@idE, @Semester,@IdStudy,@sd)";
                        //com.Parameters.AddWithValue("idE", idEnrollment + 1);
                        //com.Parameters.AddWithValue("Semester", 1);
                        //com.Parameters.AddWithValue("IdStudy", idStudies);
                        //com.Parameters.AddWithValue("sd", DateTime.Now.ToString());

                        //var dr6 = com.ExecuteNonQuery();

                    }
                    else
                    {

                        result1 = fromEnr.Select(e => e.IdEnrollment).First();
                    }






                var dont2 = _context.Student.Select(e => new { e.IndexNumber }).Where(d => d.IndexNumber.Equals(student.IndexNumber)).FirstOrDefault();

                //com.CommandText = "SELECT * FROM Student WHERE IndexNumber =@indexs";
                //com.Parameters.AddWithValue("indexs", student.IndexNumber);
                //var dr5 = com.ExecuteReader();

                if (dont2!=null)
                    {

                        return null;

                    }



                    //com.Parameters.AddWithValue("IdStuds", idStudies);

                    _context.Add(new Entities.Student()
                    {

                        FirstName = student.Firstname,
                        IndexNumber = student.IndexNumber,
                        LastName = student.Lastname,
                        BirthDate = DateTime.Now,
                        IdEnrollment = result1

                    });

                    _context.SaveChanges();

                    //com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName,Birthdate,IdEnrollment) VALUES (@Indexn,@FirstName, @LastName,@Birthdate,@Studies)";
                    ////...
                    //com.Parameters.AddWithValue("FirstName", student.Firstname);
                    //com.Parameters.AddWithValue("Indexn", student.IndexNumber);
                    //com.Parameters.AddWithValue("LastName", student.Lastname);
                    //com.Parameters.AddWithValue("Birthdate", DateTime.Parse(student.BirthDate));
                    //com.Parameters.AddWithValue("Studies", idEnrollment);

                    ////...
                    //com.ExecuteNonQuery();


                   var hello =_context.Enrollment.Select(e => e).Where(e => e.IdStudy==idStudies && e.Semester==1 && e.StartDate== _context.Enrollment.Where(e => e.IdStudy==idStudies).Max(e => e.StartDate));

                    //com.CommandText = "Select * from enrollment where idstudy=@iDstuds and semester=1 and StartDate=(select max(StartDate) from Enrollment where IdStudy=@IdStuds)";
                    //tran.Commit();
                    //var dr4 = com.ExecuteReader();


                    var enrollment = new Models.Enrollment();

                    
                    
                   
                    
                        enrollment.Idenrollment = hello.First().IdEnrollment;
                        enrollment.semester = hello.First().Semester;
                        enrollment.IdStudy = hello.First().IdStudy;
                      enrollment.StartDate   =  hello.First().StartDate.ToString();
                       

                    

                  


                    var er = new EnrollmentResponse(enrollment);

                    return er;
                }
            
        }

        public async Task<Models.Student> GetStudentByIndexAsync(string index)
        {

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())
            {

                com.CommandText = "Select * from student where IndexNumber=@index ";
                com.Parameters.AddWithValue("index", index);

                using (var reader = await com.ExecuteReaderAsync())
                {

                    if (await reader.ReadAsync())
                    {

                        var student = new Models.Student();

                        student.BirthDate = reader["Birthdate"].ToString();
                        student.Firstname = reader["FirstName"].ToString();
                        student.Lastname = reader["LastName"].ToString();
                        student.IdStudent = (int)reader["IdEnrollment"];

                        return student;

                    }
                    else
                    {
                        return null;

                    }
                }
            }
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {


               


                com.CommandText = "select * from Salt where saltID=@id";


                com.Parameters.AddWithValue("id", 1);

                client.Open();

                com.Connection = client;


                var dr = com.ExecuteReader();


                dr.Read();


                var saltc = dr["salt"].ToString();

                loginRequest.password = PasswordGenerator.Create(loginRequest.password, saltc);
                

                com.CommandText = "select * from student where Indexnumber=@index and password=@pass";


                com.Parameters.AddWithValue("pass", loginRequest.password);

                com.Parameters.AddWithValue("index", loginRequest.login);



                dr.Close();

                var dr2 = com.ExecuteReader();

                if (!dr2.Read())
                {
                    return null;
                }

                var response = new LoginResponse();
                response.login = dr2["IndexNumber"].ToString();

                response.name = dr2["LastName"].ToString();


                return response;


            }

        }

        public PromoteResponse PromoteStudents(PromoteRequest request)
        {

            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {

                client.Open();
                com.Connection = client;


                com.CommandText = "select * from Enrollment,Studies where Enrollment.IdStudy=Studies.IdStudy and Enrollment.semester=@semester and Studies.Name=@Studies";
                com.Parameters.AddWithValue("semester", request.Semester);
                com.Parameters.AddWithValue("Studies", request.Studies);

                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    return null;
                }

                dr.Close();

                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.CommandText = "Promotion";
                com.ExecuteNonQuery();


                com.CommandType = System.Data.CommandType.Text;


                com.CommandText = "select * from Enrollment,Studies where Enrollment.IdStudy=Studies.IdStudy and Name=@Studies and Semester=@semestern";

                com.Parameters.AddWithValue("semestern", request.Semester + 1);

                var dr2 = com.ExecuteReader();


                dr2.Read();

                var enrollment = new Models.Enrollment();

                enrollment.IdStudy = (int)dr2["IdStudy"];
                enrollment.semester = (int)dr2["Semester"];
                var StartDate = dr2["StartDate"];
                enrollment.StartDate = StartDate.ToString();

                var promotion = new PromoteResponse(enrollment);

                return promotion;
            }
        }

        public void SaveLogData(string data)
        {
            try
            {

                using (StreamWriter w = File.AppendText("C:\\Users\\virion\\Desktop\\apbd3\\apbd3\\apbd3\\apbd3\\Log.txt"))
                {


                    w.Write(data);

                    w.Flush();
                    w.Close();

                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);



            }
        }

        public void SaveToken(string login, string name, string token)
        {
            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {

                client.Open();
                com.Connection = client;


                com.CommandText = "insert into RefreshToken (Login,Name,RefreshToken) values (@login,@name,@token)";
                com.Parameters.AddWithValue("login", login);
                com.Parameters.AddWithValue("name", name);
                com.Parameters.AddWithValue("token", token);

                com.ExecuteNonQuery();

            }
        }



        public TokenResponse CheckToken(string token) 
        {



            using (var client = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            using (var com = new SqlCommand())

            {

                client.Open();
                com.Connection = client;


                com.CommandText = "select * from RefreshToken where RefreshToken.RefreshToken=@token ";
                com.Parameters.AddWithValue("token", token);


                var dr = com.ExecuteReader();


                if (!dr.Read())
                {


                    return null;
                }
                else

                {

                    var response = new TokenResponse();

                    response.login = dr["Login"].ToString();
                    response.name = dr["Name"].ToString();

                    return response;
        
                } 

            }


        }
    }
} 
