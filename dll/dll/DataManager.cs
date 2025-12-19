using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll
{
    public static class DataManager
    {
        private static string connectionString = "mongodb://root:password@localhost:1500/?authSource=admin";
        private static string databaseName = "shop";

        public static string ConnectionString()
            => connectionString;

        public static string DatabaseName()
            => databaseName;

        public static void SetConnectionString(string connectionString)
            => DataManager.connectionString = connectionString;
    }
}
