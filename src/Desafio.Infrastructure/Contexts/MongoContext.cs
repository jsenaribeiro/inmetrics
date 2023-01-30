using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mongo2Go;
using MongoDB.Driver;
using Desafio.Domain;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Core.Configuration;

namespace Desafio.Infrastructure;

public abstract class MongoContext
{
    static MongoContext()
    {
        RegisterSerializers();
        RegisterORMMappings();
    }

    static void RegisterSerializers()
    {
        RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        RegisterSerializer(new DateOnlySerializer());
        RegisterSerializer(new TimeOnlySerializer());
    }

    static void RegisterSerializer(IBsonSerializer serializer)
    {
        var dataType = serializer.GetType();
        var notFound = BsonSerializer.LookupSerializer(dataType) is null;
        if (notFound) BsonSerializer.RegisterSerializer(dataType, serializer);
    }

    static void RegisterORMMappings()
    {
        AppDomain.CurrentDomain.GetAssemblies()
           .SelectMany(x => x.GetTypes())
           .Where(x => x.IsSubclassOf(typeof(AbstractMap<,>)))
           .Select(x => Activator.CreateInstance(x) as IMongoMap)
           .Where(x => x is not null).ToList()
           .ForEach(x => x.ConfigureMongo());
    }
}
