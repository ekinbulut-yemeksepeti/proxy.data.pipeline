using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace proxy.data.pipeline.Interfaces
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
        
        [BsonIgnore]
        ChangeStreamStatusEnum Status { get; set; }
    }
}