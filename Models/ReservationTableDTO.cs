using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    [BsonIgnoreExtraElements]
    public class ReservationTableDTO
    {
        [BsonElement("Ordername")]
        public string OrderName { get; set; }

        [BsonElement("OrderDescription")]
        public string OrderDesc { get; set; }

        [BsonElement("PhoneNO")]
        public string PhoneNo { get; set; }

        [BsonElement("Jwt")]
        public string Jwt { get; set; }
    }
}
