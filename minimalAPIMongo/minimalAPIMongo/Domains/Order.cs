using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

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
        [BsonElement("productsIds")]
        public List<string>? ProductsIds { get; set; }
        public List<Product>? Products { get; set; }
        [BsonElement("clientId")]
        public string? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
