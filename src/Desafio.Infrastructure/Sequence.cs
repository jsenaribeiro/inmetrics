using MongoDB.Bson;

namespace Desafio.Infrastructure;

public record Sequence(string Label, long Value)
{
   public ObjectId _id { get; set; }

   public Sequence With(long value) => this with { Value = value };
}