using System.Collections.Generic;
using apbd3.Models;

namespace apbd3.DAL
{
    public class MockDbService : IDbService
    {
        private static readonly IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student {IdStudent = 1, Firstname = "Jan", Lastname = "Kowalski"},
                new Student {IdStudent = 2, Firstname = "Maciej", Lastname = "Stryczynski"},
                new Student {IdStudent = 3, Firstname = "Kazimierz", Lastname = "Czeszynski"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}