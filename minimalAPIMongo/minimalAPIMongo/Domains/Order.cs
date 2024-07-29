using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("date")]
        public DateOnly Date { get; set; }
        [BsonElement("status")]
        public int Status { get; set; }
        [BsonElement("products")]
        public virtual List<string>? ProductIds { get; set; }
        [BsonElement("clientid")]
        public string? ClientId { get; set; }
    }
}
