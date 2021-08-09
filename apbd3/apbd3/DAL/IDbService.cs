using System.Collections.Generic;
using apbd3.Models;

namespace apbd3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}