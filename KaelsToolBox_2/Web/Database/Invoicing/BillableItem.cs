using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using KaelsToolBox_2.Web.Database.MongoDB;

namespace KaelsToolBox_2.Web.Database.Invoicing;

public record BillableItem
{
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime Date { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? ItemId { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Amount { get; set; }

    /// <summary>
    /// Amount of discount. Eg. 100.00 = $100.00 discount
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Discount_Flat { get; set; }

    /// <summary>
    /// Discount percentage. Eg. 0.10 = 10% discount
    /// </summary>
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Discount_Percent { get; set; }

    /// <summary>
    /// Total price. Discount_Percent applies first, then Discount_Flat is applied to the result. can be less then 0.00
    /// </summary>
    //[BsonIgnore]
    //public decimal Total
    //{
    //    get
    //    {
    //        Item item = Item.GetItem(ItemId!)!;
    //        return ((item.Price * Amount) * (1 - Discount_Percent)) - Discount_Flat;
    //    }
    //}

    //[BsonIgnore]
    //public Item Item => Connection.Instance?.Get<Item>()!;
}


