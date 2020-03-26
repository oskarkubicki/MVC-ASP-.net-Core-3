using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}