using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using KaelsToolBox_2.Web.Database.MongoDB;

namespace KaelsToolBox_2.Web.Database.Invoicing;

public record Provider : DatabaseObject
{
    [BsonRepresentation(BsonType.String)]
    public string? Name { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? ABN { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? BSB { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? ACC { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? Address_L1 { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? Address_L2 { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? PhoneNumber { get; set; }
}
