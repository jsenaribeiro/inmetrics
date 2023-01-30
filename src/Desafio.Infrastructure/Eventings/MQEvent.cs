using System;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Desafio.Domain;

namespace Desafio.Infrastructure;

public record MQEvent
{
    public MQEvent() { }

    public MQEvent(IEvent @event)
    {
        Data = @event ?? throw new ArgumentNullException(nameof(@event));
        Type = @event!.GetType()!.AssemblyQualifiedName!;
    }

    public dynamic Data { get; set; }

    public string Type { get; set; }

    public byte[] Serialize()
    {
        var jsonfied = ToJsonString();
        var binaries = Encoding.UTF8.GetBytes(jsonfied);

        return binaries;
    }

    public static IEvent Deserialize(string json)
    {
        var data = JsonSerializer.Deserialize<MQEvent>(json);
        var type = System.Type.GetType(data!.Type);

        return JsonSerializer.Deserialize(data.Data, type);
    }

    public static IEvent Deserialize(byte[] bytes)
    {
        var json = Encoding.UTF8.GetString(bytes);

        return Deserialize(json);
    }

    public string ToJsonString() => JsonSerializer.Serialize(this);
}