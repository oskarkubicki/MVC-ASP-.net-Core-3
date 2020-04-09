using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.DTO
{
    public class PromoteResponse
    {

        private Enrollment enrollment;

       public  PromoteResponse(Enrollment enrollment) {


            this.enrollment = enrollment;
        }
    }
}
