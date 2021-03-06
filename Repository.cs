﻿using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Diary.Models;
using System.Windows;

namespace Diary
{
    public class Repository
    {
        // Właściwie powinien być obiekt groupWrapper
        // ale aby uniknąć pisania konwerterów
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

                

               // dzięki typowi Querable zapytanie nie zostanie od razu wykonane
                var students = context
                    .Students
                    .Include(x => x.Group)
                    .Include(x => x.Ratings)
                    .AsQueryable();

                if (groupId != 0)
                    students = students.Where(x => x.GroupId == groupId).AsQueryable();

                // w tym miejscu wykonamy kwerendę
                // gdyby powyżej było ToList() zapytanie wywołało by się 2 razy

                // musimy przekonwertować na StudentWrapper
                // poniżej przykładowo przekonwertowany pojedynczy student
                // var student = students.First().ToWrapper();

                // musimy przekonwertować na StudentWrapper całą listę
                // czyli chcemy wywołać tę metodę dla każdego studenta z listy
                return students
                    .ToList()
                    .Select(x => x.ToWrapper())
                    .ToList();
                

            }
        }

        internal void DeleteStudent(int id)
        {
            using (var context = new ApplicationBbContext())
            {
                // var studentToDelete = context.Students.Where(x => x.Id == id).ToList();
                var studentToDelete = context.Students.Find(id);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        internal void UpdateStudent(StudentWrapper studentWrapper)
        {
            // konwersja z Wrapperów na obiekty domenowe
            var student = studentWrapper.toDao();
            var ratings = studentWrapper.ToRatingDao();
            using (var context = new ApplicationBbContext())
            {
                UpdateStudentProperities(studentWrapper, student, context);

                // aktualizacja ocen
                // pobieramy stare oceny ucznia zapisane w aktualnie w bazie
                List<Rating> studentRatings = GetStudentRaitings(student, context);

                // foreach (var item in Enum.GetNames(typeof(Subject)))
                // foreach (WorkingDays item in Enum.GetValues(typeof(WorkingDays)))

                
                UpdateRate(student, ratings, context, studentRatings, Subject.Math);
                UpdateRate(student, ratings, context, studentRatings, Subject.Technology);
                UpdateRate(student, ratings, context, studentRatings, Subject.Physics);
                UpdateRate(student, ratings, context, studentRatings, Subject.PolishLang);
                UpdateRate(student, ratings, context, studentRatings, Subject.ForeignLang);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Błąd zapisu do bazy");
                    return; 
                }
                

            }
        }

        private static List<Rating> GetStudentRaitings(Student student, ApplicationBbContext context)
        {
            return context
                .Ratings
                .Where(x => x.StudentId == student.Id)
                .ToList();
        }

        private static void UpdateStudentProperities(StudentWrapper studentWrapper, Student student, ApplicationBbContext context)
        {
            // najpierw pobieramy studenta, którego chcemy aktualizować 
            var studentToUpdate = context.Students.Find(student.Id);

            // aktualizujemy obiekt Dao => Dao
            studentToUpdate.Activities = student.Activities;
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.Comments = student.Comments;
            studentToUpdate.GroupId = student.GroupId;
        }

        /// <summary>
        /// Aktualizacja ocen
        /// </summary>
        /// <param name="student"></param>
        /// <param name="ratings"></param>
        /// <param name="context"></param>
        /// <param name="studentRatings"></param>
        /// <param name="subject"></param>
        private static void UpdateRate(Student student, List<Rating> ratings,
            ApplicationBbContext context, List<Rating> studentRatings, Subject  subject)
        {
            // musimy je porównac z nowymi ocenami
            // musimy to podzielić na kilka etapów
            // obecne oceny
            var oldRatings = studentRatings
                .Where(x => x.SubjectId == (int)subject)
                .Select(x => x.Rate).ToList();
            
            //nowe oceny 
            var newRatings = ratings
                .Where(x => x.SubjectId == (int)subject)
                .Select(x => x.Rate).ToList();

            //do usunięcia są w starej nie ma w nowej
            //var ratingsToDelete = oldRatings.Except(newRatings).ToList();
            var ratingsToDelete = GetSubRatingsToDelete(oldRatings, newRatings).ToList();
            
            // dododania
            //var ratingsToAdd = newRatings.Except(oldRatings).ToList();
            var ratingsToAdd = GetSubRatingsToAdd(oldRatings, newRatings).ToList();



            int bi = 1;

            ratingsToDelete.ForEach(x =>
            {

                // szukamy ocen które mają taką samą wartość,
                // są przypisane do tego studenta
                // oraz są z konkretnego przedmiotu
                var ratingToDelete = context.Ratings.First(y =>
                y.Rate == x &&
                y.StudentId == student.Id &&
                y.SubjectId == (int)subject);

                context.Ratings.Remove(ratingToDelete);
            });

            ratingsToAdd.ForEach(x =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = x,
                    StudentId = student.Id,
                    SubjectId = (int)subject,

                };

                context.Ratings.Add(ratingToAdd);
            });
        }


        /// <summary>
        /// Dodawanie studenta
        /// </summary>
        /// <param name="studentWrapper"></param>
        internal void AddStudent(StudentWrapper studentWrapper)
        {
            // konwersja z Wrapperów na obiekty domenowe
            var student = studentWrapper.toDao();
            var ratings = studentWrapper.ToRatingDao();
            // teraz mamy już obiekty domenowe
            using (var context = new ApplicationBbContext())
            {
                // najpierw zapisujemy do bazy aby później odczytać jakie Id nadała baza
                var dbStudent = context.Students.Add(student);

                ratings.ForEach(x =>
                {
                    // najpierw przypisujemy Id nadane przez bazę
                    x.StudentId = dbStudent.Id;
                    // później zapisujemy do bazy
                    context.Ratings.Add(x);
                });

                context.SaveChanges();

            }
        }

        private static List<int> GetSubRatingsToAdd(
            IEnumerable<int> oldSubRatings, IEnumerable<int> newSubRatings)
        {
            var subRatingsToAdd = new List<int>();
            var oldListCopy = new List<int>(oldSubRatings);
            foreach (var item in newSubRatings)
            {
                var itemInOldList = oldListCopy.FirstOrDefault(x => x == item);
                if (itemInOldList != 0)
                    oldListCopy.Remove(itemInOldList);
                else
                    subRatingsToAdd.Add(item);
            }
            return subRatingsToAdd;
        }

        private static List<int> GetSubRatingsToDelete(
            IEnumerable<int> oldSubRatings, IEnumerable<int> newSubRatings)
        {
            var subRatingsToDelete = new List<int>();
            var newListCopy = new List<int>(newSubRatings);
            foreach (var item in oldSubRatings)
            {
                var itemInNewList = newListCopy.FirstOrDefault(x => x == item);
                if (itemInNewList != 0)
                    newListCopy.Remove(itemInNewList);
                else
                    subRatingsToDelete.Add(item);
            }
            return subRatingsToDelete;
        }

    }
}
