using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using apbd3.DTO;
using apbd3.Models;
using apbd3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace apbd3.Controllers
{
    [Route("api/enrollment")]
    [ApiController]

    [Authorize(Roles = "employee")]
    public class EnrollmentsController : ControllerBase
    {

        readonly IStudentsDbService _service;

        public IConfiguration Configuration { get; set; }


        public EnrollmentsController(IStudentsDbService service, IConfiguration configuration)
        {
            _service = service;

            Configuration = configuration;


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

        [AllowAnonymous]


        public IActionResult Login(LoginRequest login)
        {

            var response = _service.Login(login);



            var Cliams = new[] {

                new Claim(ClaimTypes.NameIdentifier,response.login),
                new Claim(ClaimTypes.Name,response.name),
                new Claim(ClaimTypes.Role,"employee") };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



            var token = new JwtSecurityToken(

                issuer: "Oskar",
                audience: "employee",
                claims: Cliams,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds

                );

            var refreshToken = Guid.NewGuid();


            _service.SaveToken(response.login, response.name, refreshToken.ToString());




            return Ok(new
            {


                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken

            });

        }



        [HttpPost("promotions")]
        public IActionResult promoteStudent(PromoteRequest promotion)
        {


            var enrollment = _service.PromoteStudents(promotion);


            if (enrollment != null)
            {

                return new ObjectResult(enrollment) { StatusCode = StatusCodes.Status201Created };
            }


            else
            {
                return BadRequest("not found");
            }


        }







        [HttpPost("refresh-token/{token}")]


        public IAction RefreshToken(string requestToken)

        {


            var data = _service.CheckToken(requestToken);

            if(data!=null){



                var Cliams = new[] {

                new Claim(ClaimTypes.NameIdentifier,data.login),
                new Claim(ClaimTypes.Name,data.name),
                new Claim(ClaimTypes.Role,"employee") };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



                var token = new JwtSecurityToken(

                    issuer: "Oskar",
                    audience: "employee",
                    claims: Cliams,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds

                    );

                var refreshToken = Guid.NewGuid();


                _service.SaveToken(data.login, data.name, refreshToken.ToString());




                return Ok(new
                {


                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken

                });



            }
            else
            {

                return BadRequest("Invalid Tokens")
            }






        }
    }
}



