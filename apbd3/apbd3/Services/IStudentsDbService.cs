using apbd3.DTO;
using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.Services
{
    interface IStudentsDbService
    {
        void EnrollStudent(Student student);

        void PromoteStudents(PromoteRequest request);
    }
}
