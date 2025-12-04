using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public static class DataManager
    {
        private const string connectionString = "mongodb://root:password@localhost:1500/?authSource=admin";
        private const string databaseName = "shop";

        public static string ConnectionString()
            => connectionString;

        public static string DatabaseName()
            => databaseName;
    }
}
