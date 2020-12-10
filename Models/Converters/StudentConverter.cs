using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diary.Models.Converters
{

    // do konwersji będziemy uzywać metody rozszerzające
    // metoda rozszerzająca musi być statyczna, musi być zawarta w statycznej klasie 



    /// <summary>
    /// Metoda rozszerzająca klasę string 
    /// przykładowe wywołanie
    /// var napis = "test".ToUpper().Replace("T", "123").Trim().PadRight(20, '0').Add3x();
    /// </summary>
    public static class StringExtensions
    {
        public static string Add3x(this string model )
        {
            return model+"_3x";
        }
    }



    public static class StudentConverter
    {

        /// <summary>
        /// Konwersja obiektu Student na StudentWrapper
        /// Metoda rozszerzająca klasę Student ToWrapper()
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Konwersja obiektu StudentWrapper na Student
        /// Metoda rozszerzająca klasę StudentWrapper ToDao()  Data Access Object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Student toDao(this StudentWrapper model )
        {
            return new Student
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Comments = model.Comments,
                Activities = model.Activities,
                GroupId = model.Group.Id,

            };
        }

        /// <summary>
        /// Konwertujemy 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<Rating> ToRatingDao(this StudentWrapper model)
        {
            var ratings = new List<Rating>();

            

            // kowertujemy string na listę za pomocą metody split
            // chcemy wszystkie wartości które są po przecinku oddzielić od siebie
            // i zamienić na listę stringów
            // ForEach() - dla każdej oceny chcemy mieć nowy rekord obiekt klasy Rating
           
            if (!string.IsNullOrWhiteSpace(model.Math))
                model.Math.Split(',').ToList().ForEach(x =>
                ratings.Add(
                   new Rating
                   {
                       Rate = int.Parse(x),
                       StudentId =  model.Id,
                       SubjectId = (int)Subject.Math
                   }));

            if (!string.IsNullOrWhiteSpace(model.Physics))
                model.Physics.Split(',').ToList().ForEach(x =>
                ratings.Add(
                   new Rating
                   {
                       Rate = int.Parse(x),
                       StudentId = model.Id,
                       SubjectId = (int)Subject.Physics
                   }));

            if (!string.IsNullOrWhiteSpace(model.Technology))
                model.Technology.Split(',').ToList().ForEach(x =>
                ratings.Add(
                   new Rating
                   {
                       Rate = int.Parse(x),
                       StudentId = model.Id,
                       SubjectId = (int)Subject.Technology
                   }));

            if (!string.IsNullOrWhiteSpace(model.PolishLang))
                model.PolishLang.Split(',').ToList().ForEach(x =>
                ratings.Add(
                   new Rating
                   {
                       Rate = int.Parse(x),
                       StudentId = model.Id,
                       SubjectId = (int)Subject.PolishLang
                   }));

            if (!string.IsNullOrWhiteSpace(model.ForeignLang))
                model.ForeignLang.Split(',').ToList().ForEach(x =>
                ratings.Add(
                   new Rating
                   {
                       Rate = int.Parse(x),
                       StudentId = model.Id,
                       SubjectId = (int)Subject.ForeignLang
                   }));

            

            return ratings;
        }

    }
}
