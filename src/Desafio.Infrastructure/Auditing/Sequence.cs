using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Infrastructure;

public class Sequence
{
    public Sequence() { }

    public Sequence(string label, long value)
    {
        Label = label;
        Value = value;
    }

    public ObjectId _id { get; set; }

    public string Label { get; set; }

    public long Value { get; set; }

    public Sequence With(long value)
    {
        Value = value;
        return this;
    }
}
