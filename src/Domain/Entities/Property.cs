using MongoDB.Bson.Serialization.Attributes;
using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

[BsonIgnoreExtraElements]
public class Property : Entity
{
    [BsonElement("Name")]
    public string Name { get; private set; } = string.Empty;
    
    [BsonElement("Address")]
    public string Address { get; private set; } = string.Empty;
    
    [BsonElement("Price")]
    [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
    public decimal Price { get; private set; }
    
    [BsonElement("CodeInternal")]
    public string CodeInternal { get; private set; } = string.Empty;
    
    [BsonElement("Year")]
    public int Year { get; private set; }
    
    [BsonElement("IdOwner")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string IdOwner { get; private set; } = MongoDB.Bson.ObjectId.Empty.ToString();

    public Property(string name, string address, decimal price, string codeInternal, int year, string idOwner)
    {
        Name = name;
        Address = address;
        Price = price;
        CodeInternal = codeInternal;
        Year = year;
        IdOwner = idOwner;
    }

    // For MongoDB serialization
    private Property() { }
}