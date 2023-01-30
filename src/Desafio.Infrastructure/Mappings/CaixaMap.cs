using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Desafio.Domain;

namespace Desafio.Infrastructure;

public class CaixaMap : AbstractMap<Caixa, Guid>
{
    public override void EntityFrameworkMap(EntityTypeBuilder<Caixa> builder)
    {
        builder.Property(x => x.Data).HasColumnName("data").AsDateOnly();
        builder.Navigation(x => x.Lancamentos).AutoInclude();
    }

    public override void MongoMap(BsonClassMap<Caixa> mapper)
    {
        mapper.MapMember(x => x.Data).AsDateOnly();
    }
}