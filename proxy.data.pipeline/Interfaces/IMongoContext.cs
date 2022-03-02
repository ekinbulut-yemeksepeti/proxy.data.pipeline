using MongoDB.Driver;

namespace proxy.data.pipeline.Interfaces
{
    public interface IMongoContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}