using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class UserRegisterDTO
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("emailid")]
        public string UserEmail { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
