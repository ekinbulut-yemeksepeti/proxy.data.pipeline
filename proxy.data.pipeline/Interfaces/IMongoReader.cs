using MongoDB.Driver;

namespace proxy.data.pipeline.Interfaces
{
    public interface IMongoReader
    {
        MongoClientSettings ReadSettings();
    }
}