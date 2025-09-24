using MongoDB.Bson.Serialization.Attributes;
using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

public class Property : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string CodeInternal { get; private set; } = string.Empty;
    public int Year { get; private set; }
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string IdOwner { get; private set; } = string.Empty;

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