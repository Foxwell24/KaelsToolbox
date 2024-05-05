using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using KaelsToolBox_2.Web.Database.MongoDB;

namespace KaelsToolBox_2.Web.Database.Invoicing;

public record Client : DatabaseObject
{
    [BsonRepresentation(BsonType.String)]
    public string? Name { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? Email { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? PlanManager { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? AccessNumber { get; set; }
}
