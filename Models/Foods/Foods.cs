using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Foods
{
    [BsonIgnoreExtraElements]
    public class Foods
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("foodname")]
        public string FoodName { get; set; }

        [BsonElement("price")]
        public int Pricing { get; set; }

        [BsonElement("image")]
        public string ImgaeUrl { get; set; }

        [BsonElement("fdesc")]
        public string FoodDesc { get; set; }

        [BsonElement("Quantity")]
        public int FoodQty { get; set; } 

        public Foods()
        {
            FoodQty = 0;
        }

    }
}
