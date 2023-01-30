using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Desafio.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Desafio.Application;

public static class RabbitMqExtensions
{
    private static IModel? _channel;

    public static void UseRabbitMQ(this WebApplication app, string topic, bool ignoreWhenOffline = false)
    {
        Task.Factory.StartNew(() => OnListen(app, topic, ignoreWhenOffline));
        EventBus.Dispatch = evt => OnPublish(evt, topic, ignoreWhenOffline);
    }

    private static void OnPublish(MQEvent e, string topic, bool ignoreWhenOffline)
    {
        var channel = ConnectChannel(topic, ignoreWhenOffline);
        if (channel is null) return;

        var (message, routing) = (e.Serialize(), string.Empty);

        channel.BasicPublish(topic, routing, null, message);
    }

    private static void OnListen(WebApplication app, string topic, bool ignoreWhenOffline)
    {
        var connection = ConnectChannel(topic, ignoreWhenOffline);
        if (connection is null) return;

        var consumer = new EventingBasicConsumer(connection);
        var queue = connection.QueueDeclare().QueueName;

        connection.QueueBind(queue, topic, string.Empty);
        consumer.Received += async (_, arg) => await EventBus.ListenAsync(app, arg.Body.ToArray());
        connection.BasicConsume(queue, true, consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static IModel ConnectChannel(string topic, bool ignoreWhenOffline)
    {
        try
        {
            if (_channel is not null) return _channel;

            var hostname = Environment.GetEnvironmentVariable("MQ_HOST") ?? "localhost";
            var hostport = int.Parse(Environment.GetEnvironmentVariable("MQ_PORT") ?? "5672");
            var factory = new ConnectionFactory { HostName = hostname, Port = hostport };

            _channel = factory.CreateConnection().CreateModel();
            _channel.ExchangeDeclare(topic, ExchangeType.Fanout);

            return _channel;
        }
        catch (Exception)
        {
            if (!ignoreWhenOffline) throw;
            else return default(IModel);
        }
    }
}