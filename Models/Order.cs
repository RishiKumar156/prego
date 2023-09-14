using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("jwt")]
        public string Jwt { get; set; }

        [BsonElement("Ordername")]
        public string OrderName { get; set; }

        [BsonElement("FoodName")]
        public string FoodName { get; set; }

        [BsonElement("totalPrice")]
        public string TotlaPrice { get; set; }
    }
}
