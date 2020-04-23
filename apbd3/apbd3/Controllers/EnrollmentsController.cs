using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using apbd3.DTO;
using apbd3.Models;
using apbd3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd3.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        readonly IStudentsDbService _service;

        //Constructor injection (SOLID - D - Dependency Injection)
        public EnrollmentsController(IStudentsDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {



            var enrollment = _service.EnrollStudent(student);

            if (enrollment != null)
            {
                return new ObjectResult(enrollment) { StatusCode = StatusCodes.Status201Created };

            }
            else

            {
                return BadRequest("bad request");
            }

        }

        [HttpGet]

        public IActionResult Login(LoginRequest login)


        {

            var Cliams = new[] {

                new Claim(ClaimTypes.NameIdentifier)
            
            
            
            
            };





        }




        [HttpPost("promotions")]
        public IActionResult promoteStudent(PromoteRequest promotion)
        {


            var enrollment= _service.PromoteStudents(promotion);


            if (enrollment != null) { 
            
            return new ObjectResult(enrollment) { StatusCode = StatusCodes.Status201Created };
            }


            else
            {
                return BadRequest("not found");
            }


            }



        }
}



