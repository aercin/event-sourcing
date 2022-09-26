using core_domain.Abstractions;
using MongoDB.Bson.Serialization.Attributes;

namespace core_domain.Entities
{
    [BsonIgnoreExtraElements]
    public class MongoBaseEntity : IAggregateRoot
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
