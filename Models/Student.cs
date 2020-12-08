using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public string Match { get; set; }
        public string Technology { get; set; }
        public string Physics { get; set; }
        public string PolishLang { get; set; }
        public string ForeignLang { get; set; }
        public string Activities { get; set; }
        public Group Group { get; set; }
    }
}
