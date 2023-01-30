namespace Desafio.Infrastructure;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

internal class TimeOnlySerializer : StructSerializerBase<TimeOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
    {
        var dateTime = new DateTime(value.ToTimeSpan().Ticks);
        var ticks = BsonUtils.ToMillisecondsSinceEpoch(dateTime);
        context.Writer.WriteDateTime(ticks);
    }

    public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var ticks = context.Reader.ReadDateTime();
        var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(ticks);
        return new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second);
    }
}

public static class TimeOnlySerializerExtensions
{
    public static void AsTimeOnly(this BsonMemberMap map) =>
       map.SetDefaultValue(new TimeOnly())
          .SetSerializer(new TimeOnlySerializer());
}