using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using KaelsToolBox_2.Web.Database.MongoDB;

namespace KaelsToolBox_2.Web.Database.Invoicing;

public record Invoice : DatabaseObject
{
    [BsonRepresentation(BsonType.Int32)]
    public int InvoiceNumber { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? ProviderId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? CustomerId { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime Date { get; set; }

    public List<BillableItem> Bills = [];
}

