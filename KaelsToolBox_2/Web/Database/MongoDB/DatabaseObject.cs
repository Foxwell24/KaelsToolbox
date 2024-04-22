using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web.Database.MongoDB;

public record DatabaseObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonRepresentation(BsonType.String)]
    public string? Note { get; set; }

    [BsonRepresentation(BsonType.Boolean)]
    public bool IsDeleted { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? DateDeleted { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime DateCreated { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime LastUpdated { get; set; }
}
