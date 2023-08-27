using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class UserRegister
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("emailid")]
        public string UserEmail { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("createTime")]
        public DateTime UserCreateOn { get; set; } = DateTime.UtcNow;

        [BsonElement("authToken")]
        public string AuthToken { get; set; }
    }
}
