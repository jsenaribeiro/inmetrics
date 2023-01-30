using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Desafio.Domain;

namespace Desafio.Infrastructure;

public interface IMongoMap { void ConfigureMongo(); }

public abstract class AbstractMap<E, I> : IEntityTypeConfiguration<E>, IMongoMap where E : class, IEntity<I> where I : IEquatable<I>
{
    private readonly bool autonumeric;
    private readonly string? tableName;

    protected AbstractMap(bool isAutoNumericId = false) => this.autonumeric = isAutoNumericId;

    protected AbstractMap(string tableName) => this.tableName = tableName;

    protected AbstractMap(string tableName, bool isAutoNumericId)
    {
        this.tableName = tableName;
        this.autonumeric = isAutoNumericId;
    }

    public void Configure(EntityTypeBuilder<E> builder)
    {
        var name = tableName ?? typeof(E).Name.ToLower() + "s";

        builder.HasKey(x => x.Id);
        builder.ToTable(name);

        if (autonumeric) builder
           .Property(x => x.Id)
           .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.Audit, audit =>
        {
            audit.Property(x => x.CRUD).HasColumnName("audit.crud");
            audit.Property(x => x.User).HasColumnName("audit.user");
            audit.Property(x => x.Date).HasColumnName("audit.date");
        });

        EntityFrameworkMap(builder);
    }

    public void ConfigureMongo()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(E)))
        {
            BsonClassMap.RegisterClassMap<E>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(x => x.Id);

                if (typeof(I).Name == nameof(Guid)) cm.IdMemberMap
                .SetSerializer(new GuidSerializer(BsonType.String));

                MongoMap(cm);
            });
        }
    }

    public abstract void EntityFrameworkMap(EntityTypeBuilder<E> builder);

    public abstract void MongoMap(BsonClassMap<E> mapper);
}