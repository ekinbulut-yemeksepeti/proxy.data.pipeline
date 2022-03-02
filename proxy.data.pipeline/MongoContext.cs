using MongoDB.Driver;
using proxy.data.pipeline.Interfaces;

namespace proxy.data.pipeline
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IMongoDbSettings settings, IMongoReader mongoSettings)
        {
            var client = new MongoClient(mongoSettings.ReadSettings());
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}