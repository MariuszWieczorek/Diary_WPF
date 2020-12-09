using Diary.Models.Configurations;
using Diary.Models.Domains;
using System;
using System.Data.Entity;
using System.Linq;

namespace Diary
{
    public class ApplicationBbContext : DbContext
    {
         public ApplicationBbContext()
            : base("name=ApplicationBbContext")
        {
        }

        // wskazujemy klasy domenowe
        // Entity Framework wie, ¿e ma stworzyæ 3 tabele jak poni¿ej
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        // musimy przes³oniæ metodê OnModelCreating
        // aby wskazaæ, ¿e mamy pliki konfiguracyjne
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }

    }
}