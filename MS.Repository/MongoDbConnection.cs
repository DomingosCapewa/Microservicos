
using MongoDB.Driver;
using MS.Repository.Interfaces;

namespace MS.Repository
{
    public class MongoDbConnection : IMongoDbConnection
    {
        private readonly IMongoDatabase _database;

        public MongoDbConnection(string connectionString, string dbName)
        {
            try
            {
               var client = new MongoClient(connectionString);
               _database = client.GetDatabase(dbName);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not initialize MongoDB connection.", ex);
            }
        }

        public IMongoDatabase GetDatabase() => _database;
    }
}