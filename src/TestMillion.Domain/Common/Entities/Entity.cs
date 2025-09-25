using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestMillion.Domain.Common.Entities;

public abstract class Entity : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; protected set; } = ObjectId.GenerateNewId().ToString();
}