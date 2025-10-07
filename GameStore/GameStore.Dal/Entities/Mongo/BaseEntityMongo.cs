using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.Dal.Entities.Mongo
{
    public class BaseEntityMongo
    {
        [BsonId]
        public ObjectId _id { get; set; }
    }
}
