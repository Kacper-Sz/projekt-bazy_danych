using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll
{
    public class MongoDbManager
    {
        private readonly IMongoDatabase _database;

        public MongoDbManager(string connectionString, string databaseName)
        {
            MongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database => _database;
    }
}
