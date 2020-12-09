using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Converters
{
    
    // tu będziemy uzywać metody rozszerzające
    // metoda rozszerzająca musi być statyczna, musi być zawarta w klasie statycznej

    public static class StringExtensions
    {
        public static string ReturnOne(this string model )
        {
            return "1";
        }
    }



    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student model)
        {
            return new StudentWrapper();
        }
    }
}
