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

public class LancamentoMap : AbstractMap<Lancamento, Guid>
{
    public override void EntityFrameworkMap(EntityTypeBuilder<Lancamento> builder)
    {
        builder.Property(x => x.DataHora).HasColumnName("data");
        builder.Property(x => x.Valor).HasColumnName("valor");
    }

    public override void MongoMap(BsonClassMap<Lancamento> mapper) { }
}