using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Shop.Domain.Models.Mongo
{
    public class MongoOrder
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("user_id")]
        public int UserId { get; set; }

        [BsonElement("products")]
        public List<MongoProductItem> Products { get; set; } = new List<MongoProductItem>();

        [BsonElement("address")]
        public MongoAddress Address { get; set; } = null!;
    }

    public class MongoProductItem
    {
        [BsonElement("product_id")]
        public int ProductId { get; set; }

        [BsonElement("qty")]
        public int Qty { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    }

    public class MongoAddress
    {
        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("street")]
        public string Street { get; set; } = string.Empty;
    }
}
