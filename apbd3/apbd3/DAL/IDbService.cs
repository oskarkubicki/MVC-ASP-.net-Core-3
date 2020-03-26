using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.DAL
{
    interface IDbService
    {


        public IEnumerable<Student> GetStudents();
    }
}
