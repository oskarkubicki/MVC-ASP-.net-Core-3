using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd3.Models;
using Microsoft.AspNetCore.Mvc;

namespace apbd3.Controllers


{

    [ApiController]
    [Route("api/students")]
 
    public class StudentsController : ControllerBase
    {   [HttpGet]
        public string GetStudents(string orderBy)
        {
            return $"Kowalski,Malewski,Andrzejewski sorted by={orderBy}";
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


        public IActionResult createStudent (Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }



       [HttpPut]


       public IActionResult putStudent(Student student) 
        
        
        {


            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok("Update complete");
        }



        [HttpDelete]


        public IActionResult deleteStudent(Student student)


        {


           
            return Ok("Delete complete");
        }

    }
}