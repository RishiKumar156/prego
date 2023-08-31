using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class Email
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("Guid")]
        public Guid SessionGuid { get; set; } = Guid.NewGuid();

        [BsonElement("CreatedOn")]
        public DateTime UserRegisteredOn { get; set; } = DateTime.UtcNow;

    }
}
