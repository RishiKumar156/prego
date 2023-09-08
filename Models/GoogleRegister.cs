using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class GoogleRegister
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("username")]
        public string Gusername { get; set; }

        [BsonElement("emailId")]
        public string GmailId { get; set; }

        [BsonElement("picture")]
        public string Picture { get; set; }

        [BsonElement("GoogleID")]
        public string Gid { get; set; }

    }
}
