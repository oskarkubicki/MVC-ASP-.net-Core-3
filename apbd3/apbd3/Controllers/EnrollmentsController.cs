using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IStudentsDbService _service;

        public EnrollmentsController(IStudentsDbService service, IConfiguration configuration)
        {
            _service = service;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            var enrollment = _service.EnrollStudent(student);

            return enrollment != null
                ? new ObjectResult(enrollment) {StatusCode = StatusCodes.Status201Created}
                : BadRequest("bad request");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest login)
        {
            var response = _service.Login(login);
            var cliams = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, response.Login),
                new Claim(ClaimTypes.Name, response.Name),
                new Claim(ClaimTypes.Role, "employee")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "Oskar",
                "employee",
                cliams,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            var refreshToken = Guid.NewGuid();

            _service.SaveToken(response.Login, response.Name, refreshToken.ToString());

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteRequest promotion)
        {
            var enrollment = _service.PromoteStudents(promotion);

            return enrollment != null
                ? new ObjectResult(enrollment) {StatusCode = StatusCodes.Status201Created}
                : BadRequest("not found");
        }

        [AllowAnonymous]
        [HttpPost("refresh-token/{token}")]
        public IActionResult RefreshToken(string token)
        {
            var data = _service.CheckToken(token);

            if (data == null) return BadRequest("Invalid Tokens");
            var cliams = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, data.Login),
                new Claim(ClaimTypes.Name, data.Name),
                new Claim(ClaimTypes.Role, "employee")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accestoken = new JwtSecurityToken(
                "Oskar",
                "employee",
                cliams,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            var refreshToken = Guid.NewGuid();

            _service.SaveToken(data.Login, data.Name, refreshToken.ToString());

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(accestoken),
                refreshToken
            });
        }
    }
}