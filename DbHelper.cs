using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public static class DbHelper
    {
        public static string ConnectionStringBuilder(string serverAddress, string serverName, string database, string user, string password)
        {
            if (String.IsNullOrWhiteSpace(serverName))
                return $@"Server={serverAddress};Database={database};Uid={user};Pwd={password};";
            else
                return $@"Server={serverAddress}\\{serverName};Database={database};Uid={user};Pwd={password};";
        }

        public static string ConnectionStringBuilder(string serverAddress, string database, string user, string password)
        {
            return $@"Server={serverAddress};Database={database};Uid={user};Pwd={password};";
        }

    }
}
