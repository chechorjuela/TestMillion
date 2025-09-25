using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace TestMillion.Infrastructure.Persistence.MongoDB.Serializers;

public class DateOnlySerializer : SerializerBase<DateOnly>
{
    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.DateTime)
        {
            var ticks = context.Reader.ReadDateTime();
            return DateOnly.FromDateTime(DateTime.UnixEpoch.AddMilliseconds(ticks));
        }
        throw new BsonSerializationException("Expected DateTime type");
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        var dateTime = value.ToDateTime(TimeOnly.MinValue);
        var unixTime = (long)(dateTime - DateTime.UnixEpoch).TotalMilliseconds;
        context.Writer.WriteDateTime(unixTime);
    }
}