using Confluent.Kafka;
using Desafio.Infrastructure;

namespace Desafio.Application;

public static class KafkaExtensions
{
    private static string connection = Environment.GetEnvironmentVariable("KAFKA_HOST") ?? "localhost:9092";

    //private static CancellationTokenSource cancellation = new CancellationTokenSource();

    public static void UseKafka(this WebApplication app, string topic, bool ignoreWhenOffline = false)
    {
        Task.Factory.StartNew(() => OnListen(app, topic, ignoreWhenOffline));
        EventBus.Dispatch = evt => OnPublish(evt, topic, ignoreWhenOffline);
    }

    private static void OnPublish(MQEvent evt, string topic, bool ignoreWhenOffline)
    {
        var settings = new ProducerConfig { BootstrapServers = connection };

        using (var producer = new ProducerBuilder<Null, string>(settings).Build())
        {
            try
            {
                var jsonfied = evt.ToJsonString();
                var message = new Message<Null, string> { Value = jsonfied };

                producer.ProduceAsync(topic, message);
                producer.Flush(TimeSpan.FromSeconds(3));

                //Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
            }
            catch (Exception)
            {
                if (!ignoreWhenOffline) throw;
            }
        }
    }

    private static void OnHandler(DeliveryReport<Null, string> report) =>
       Console.WriteLine(!report.Error.IsError
          ? $"Delivered message to {report.TopicPartitionOffset}"
          : $"Delivery Error: {report.Error.Reason}");

    private static void OnListen(WebApplication app, string topic, bool ignoreWhenOffline)
    {
        var seetings = new ConsumerConfig
        {
            GroupId = "desafio",
            BootstrapServers = connection,
            // Note: The AutoOffsetReset property determines the start offset in the event
            // there are not yet any committed offsets for the consumer group for the
            // topic/partitions of interest. By default, offsets are committed
            // automatically, so in this example, consumption will only start from the
            // earliest message in the topic 'my-topic' the first time you run the program.
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(seetings).Build())
        {
            consumer.Subscribe(topic);

            try
            {
                while (true)
                {
                    try
                    {
                        var consumed = consumer.Consume(TimeSpan.FromSeconds(3));
                        if (consumed is null) continue;


                        var @event = MQEvent.Deserialize(consumed.Message.Value);
                        if (@event is not null) EventBus.ListenAsync(app, @event).Wait();

                        Console.WriteLine($"Consumed message '{consumed.Message.Value}' at: '{consumed.TopicPartitionOffset}'.");
                        Thread.Sleep(3210);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
            catch (Exception)
            {
                if (!ignoreWhenOffline) throw;
            }
        }
    }
}
