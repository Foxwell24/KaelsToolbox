using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using KaelsToolBox_2.Web.Database.MongoDB;

namespace KaelsToolBox_2.Web.Database.Invoicing;

public record Item : DatabaseObject
{
    [BsonRepresentation(BsonType.String)]
    public string? Name { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? Code { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }
}