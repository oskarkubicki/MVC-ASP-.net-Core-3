using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.DTO
{
    public class PromoteResponse
    {
        public string Studies { get; set; }

        public int Semester { get; set; }
        public PromoteResponse(Enrollment enrollment) {
            
            Studies = enrollment.StartDate;

            Semester = enrollment.semester;
        }
    }
}
