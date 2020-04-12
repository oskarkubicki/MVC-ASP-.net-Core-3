using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.DAL
{
    public interface IDbService
    {

        public IEnumerable<Student> GetStudents();
    }
}
