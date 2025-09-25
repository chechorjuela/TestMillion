using MongoDB.Bson.Serialization.Attributes;
using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

public class PropertyTrace : Entity
{
  public DateOnly DateSale { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public decimal Value { get; private set; }
  public decimal Tax { get; private set; }
  
  [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
  public string IdProperty { get; private set; } = string.Empty;
  
  public PropertyTrace(DateOnly dateSale, string name, decimal value, decimal tax, string idProperty)
  {
      DateSale = dateSale;
      Name = name;
      Value = value;
      Tax = tax;
      IdProperty = idProperty;
  }
  
  // For MongoDB serialization
  private PropertyTrace() { }
}
