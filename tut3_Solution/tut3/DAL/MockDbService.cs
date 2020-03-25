using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut3.Models;

namespace tut3.DAL
{
    public class MockDbService : IServiceDB
    {
        private static IEnumerable<Student> _sutdentsList;

        static MockDbService()
        {
            _sutdentsList = new List<Student>
            {
                new Student{IdStudent=1, FirstName = "A man", LastName="shouldnt"},
                new Student{IdStudent=2, FirstName = "live with", LastName="mom"},
                new Student{IdStudent=3, FirstName = "in this", LastName="age"}
            };
        }
            public IEnumerable<Student> GetStudents()
            {
                return _sutdentsList;
            }

        
    }
}
