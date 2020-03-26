using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd3.DAL;
using apbd3.Models;
using Microsoft.AspNetCore.Mvc;

namespace apbd3.Controllers


{

    [ApiController]
    [Route("api/students")]

    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbservice;


        public StudentsController(IDbService dbService)
        
        {

            _dbservice = dbService;
        }
        
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbservice.GetStudents());
        }


        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {

            if (id == 1)
            {

                return Ok("Kowalski");

            }
            else if (id == 2)
            {

                return Ok("Malewski");

            }
            else {

                return NotFound("The element was not found");
            }
        }


        [HttpPost]


        public IActionResult createStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }



        [HttpPut("{Id}")]


        public IActionResult putStudent(int Id)


        {

            Student student = new Student();
            student.IdStudent = Id;
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok("Update complete");
        }



        [HttpDelete("{Id}")]


        public IActionResult deleteStudent(int Id)


        {


           
            return Ok("Delete complete");
        }

    }
}