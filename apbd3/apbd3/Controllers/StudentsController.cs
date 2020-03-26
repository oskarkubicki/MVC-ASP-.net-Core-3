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
    {
        public string GetStudent()
        {
            return "Kowalski,Malewski,Andrzejewski";
        }
    }
}