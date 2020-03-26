using apbd3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd3.DAL
{
    public class MockDbService : IDbService
    {

        private static IEnumerable<Student> _students;

        static MockDbService()
        {


            _students = new List<Student> { 
            
            new Student{IdStudent=1,Firstname="Jan",Lastname="Kowalski"},
            
            };
        }
        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
