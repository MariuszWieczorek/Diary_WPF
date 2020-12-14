using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diary
{
    public static class DbHelper
    {
        public static string ConnectionStringBuilder(string serverAddress, string serverName, string database, string user, string password)
        {
            if (String.IsNullOrWhiteSpace(serverName))
                return $@"Server={serverAddress};Database={database};Uid={user};Pwd={password};";
            else
                return $@"Server={serverAddress}\{serverName};Database={database};Uid={user};Pwd={password};";
        }

        public static string ConnectionStringBuilder(string serverAddress, string database, string user, string password)
        {
            return $@"Server={serverAddress};Database={database};Uid={user};Pwd={password};";
        }

   
        /// <summary>
        /// Testowanie połączenia SQL z użyciem podanego connection stringa
        /// </summary>
        /// <param name="testConnectionString"></param>
        /// <returns></returns>
        public static bool ConnectionSettingsTest(string testConnectionString)
        {
            using (var context = new ApplicationBbContext(testConnectionString))
            {
                try
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                }
                catch (Exception)
                {
                    //MessageBox.Show(testConnectionString);
                    return false;
                }    
                return true;
            }
        }

        /// <summary>
        /// Informacje na temat połączenia SQL'a
        /// </summary>
        /// <param name="testConnectionString"></param>
        /// <returns></returns>        
        public static bool ConnectionSettingsInfo(string testConnectionString)
        {
            using (var context = new ApplicationBbContext(testConnectionString))
            {
                try
                {
                    context.Database.Connection.Open();
                    if (context.Database.Connection.State == ConnectionState.Open)
                    {
                        string connectionInfo = 
                               "Konfiguracja połączenia SQL OK"
                            + $"\nConnectionString: {testConnectionString}"
                            + $"\nDataBase: { context.Database.Connection.Database}"
                            + $"\nDataSource: { context.Database.Connection.DataSource}"
                            + $"\nServerVersion: { context.Database.Connection.ServerVersion}"
                            + $"\nTimeOut: { context.Database.Connection.ConnectionTimeout}";

                            MessageBox.Show(connectionInfo);
                    }
                    context.Database.Connection.Close();
                }
                catch (Exception)
                {
                   return false;
                }

                return true;
            }
        }
    }
}
