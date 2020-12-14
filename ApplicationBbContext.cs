using Diary.Models.Configurations;
using Diary.Models.Domains;
using Diary.Properties;
using Diary;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Diary.Views;

namespace Diary
{
    public class ApplicationBbContext : DbContext
    {

        public static string ServerAddress   
        {
            get
            {
                return Settings.Default.ServerAddress;
            }
            set
            {
                Settings.Default.ServerAddress = value;
            }
        }

        public static string ServerName
        {
            get
            {
                return Settings.Default.ServerName;
            }
            set
            {
                Settings.Default.ServerName = value;
            }
        }

        public static string DataBase
        {
            get
            {
                return Settings.Default.Database;
            }
            set
            {
                Settings.Default.Database = value;
            }
        }


        public static string User
        {
            get
            {
                return Settings.Default.User;
            }
            set
            {
                Settings.Default.User = value;
            }
        }

        public static string Password
        {
            get
            {
                return Settings.Default.Password;
            }
            set
            {
                Settings.Default.Password = value;
            }
        }


        private static string ConnectionString
        {
            get
            {
                return $"Server={ServerAddress};Database={DataBase};Uid={User};Pwd={Password};";
            }
        }

        // ConnectionString "name=ApplicationBbContext"

        private static string _connectionString2 = "Server=127.0.0.1;Database=Diary;Uid=user1;Pwd=alamakota;";

        
       private static string _connectionString = DbHelper.ConnectionStringBuilder(ServerAddress, ServerName, DataBase, User, Password);
 
    
        public ApplicationBbContext() 
            : base(_connectionString)
        {
            if (!DbHelper.ConnectionSettingsTest(_connectionString))
            {
                var connectionConfigurationWindow = new ConnectionConfigurationView();
                connectionConfigurationWindow.ShowDialog();
            }
        }

        public ApplicationBbContext(string connectionString)
            : base(connectionString)
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