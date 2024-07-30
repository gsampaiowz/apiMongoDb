﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("cpf")]
        public string? Cpf { get; set;}

        [BsonElement("phone")]
        public string? Phone { get; set;}

        [BsonElement("address")]
        public string? Address { get; set; }

        public virtual User? User { get; set; }

    }
}
