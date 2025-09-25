using MongoDB.Bson.Serialization.Attributes;
using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

public class PropertyImage : Entity
{
    public string File { get; private set; } = string.Empty;
    public bool Enabled { get; private set; }
    
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string IdProperty { get; private set; } = string.Empty;

    public PropertyImage(string file, bool enabled, string idProperty)
    {
        File = file;
        Enabled = enabled;
        IdProperty = idProperty;
    }


    // For MongoDB serialization
    private PropertyImage() { }
}