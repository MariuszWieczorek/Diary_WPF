using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Diary
{
    public class Repository
    {
        public List<Group> GetGroups()
        {
            using (var context = new ApplicationBbContext())
            {
                return context.Groups.ToList();
            }
        }

        public List<StudentWrapper> GetStudents(int groupId = 0)
        {
            using (var context = new ApplicationBbContext())
            {

                var test = "test";
                test = test.ToUpper().ReturnOne();

               // dzięki typowi Querable zapytanie nie zostanie od razu wykonane
                var students = context
                    .Students
                    .Include(x => x.Group)
                    .Include(x => x.Ratings)
                    .AsQueryable();

                if (groupId != 0)
                    students = students.Where(x => x.GroupId == groupId);

                // w tym miejscu wykonamy kwerendę
                // gdyby powyżej było ToList() zapytanie wywołało by się 2 razy

                // musimy przekonwertować na StudentWrapper
                // poniżej przykładowo przekonwertowany pojedynczy student
                var student = students.First().ToWrapper();

                // musimy przekonwertować na StudentWrapper całą listę
                // czyli chcemy wywołać tę metodę dla każdego studenta z listy
                return students
                    .ToList()
                    .Select(x => x.ToWrapper())
                    .ToList();
                

            }
        }

    }
}
