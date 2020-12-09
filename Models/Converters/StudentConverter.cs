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
        public static string AddXXX(this string model )
        {
            return model+"_XXX";
        }
    }



    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student model)
        {
            return new StudentWrapper
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Comments = model.Comments,
                Activities = model.Activities,
                Group = new GroupWrapper
                { 
                    Id = model.Group.Id,
                    Name = model.Group.Name
                },
                // tu chcemy mieć listę ocen wyświetlaną po przecinku
                // używamy metod zamieniającej Listę na stringa
                Math = string.Join(",",model.Ratings.Where(y => y.SubjectId == (int)Subject.Math).Select(y => y.Rate)),
                Physics = string.Join(",", model.Ratings.Where(y => y.SubjectId == (int)Subject.Physics).Select(y => y.Rate)),
                Technology = string.Join(",", model.Ratings.Where(y => y.SubjectId == (int)Subject.Technology).Select(y => y.Rate)),
                PolishLang = string.Join(",", model.Ratings.Where(y => y.SubjectId == (int)Subject.PolishLang).Select(y => y.Rate)),
                ForeignLang = string.Join(",", model.Ratings.Where(y => y.SubjectId == (int)Subject.ForeignLang).Select(y=>y.Rate)),

            };
        }
    }
}
