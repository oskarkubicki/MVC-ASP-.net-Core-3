using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.DTO
{
    public class EnrollmentResponse
    {


        public EnrollmentResponse(Enrollment enrollment)
        {

            Idenrollment = enrollment.Idenrollment;
            semester = enrollment.semester;
            IdStudy = enrollment.IdStudy;

            StartDate = enrollment.StartDate;
        }

        public int Idenrollment { get; set; }

        public int semester { get; set; }

        public int IdStudy { get; set; }

        public string StartDate { get; set; }


    }
}
