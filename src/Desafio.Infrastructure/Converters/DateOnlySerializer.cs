namespace Desafio.Infrastructure;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

public class DateOnlySerializer : StructSerializerBase<DateOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        var dateTimeString = value.ToString("yyyy-MM-dd");
        context.Writer.WriteString(dateTimeString);
    }

    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.String)
        {
            var dateTimeString = context.Reader.ReadString();
            var validDateTime = DateOnly.TryParse(dateTimeString, out var result);

            return validDateTime ? result : default;
        }

        else if (context.Reader.CurrentBsonType == BsonType.DateTime)
        {
            var ticks = context.Reader.ReadDateTime();
            var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(ticks);
            return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        else if (context.Reader.State == BsonReaderState.EndOfDocument)
            context.Reader.ReadEndDocument();

        context.Reader.SkipValue();

        return default;
    }
}

public static class DateOnlySerializerExtensions
{
    public static void AsDateOnly(this BsonMemberMap map) =>
       map.SetDefaultValue(new DateOnly())
          .SetSerializer(new DateOnlySerializer());
}