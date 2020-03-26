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
            new Student{IdStudent=2,Firstname="Maciej",Lastname="Stryczynski"},
            new Student{IdStudent=3,Firstname="Kazimierz",Lastname="Czeszynski"}

            };
        }
        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
