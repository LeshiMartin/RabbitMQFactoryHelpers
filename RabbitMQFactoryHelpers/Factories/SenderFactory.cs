using RabbitMQ.Client;
using static RabbitMQFactoryHelpers.DefaultConnectionValues;


namespace RabbitMQFactoryHelpers.Factories;
public static class SenderFactory
{
  /// <summary>
  /// Generates the sender.
  /// </summary>
  /// <param name="queueName">Name of the queue.</param>
  /// <param name="hostName">Name of the host.</param>
  /// <param name="port">The port.</param>
  /// <returns></returns>
  public static IModel GenerateSender ( string queueName,
    string hostName = LOCALHOST_HOSTNAME,
    int port = LOCALHOST_PORT )
  {

    var factory = new ConnectionFactory () { HostName = hostName, Port = port };
    var connection = factory.CreateConnection ();
    var channel = connection.CreateModel ();
    channel.QueueDeclare (queue: queueName,
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    return channel;
  }

  /// <summary>
  /// Generates the sender fan out exchange.
  /// </summary>
  /// <param name="queueName">Name of the queue.</param>
  /// <param name="hostName">Name of the host.</param>
  /// <param name="port">The port.</param>
  /// <returns></returns>
  public static IModel GenerateSenderFanOutExchange ( string queueName,
    string hostName = LOCALHOST_HOSTNAME,
    int port = LOCALHOST_PORT )
  {
    var factory = new ConnectionFactory () { HostName = hostName, Port = port };
    var connection = factory.CreateConnection ();
    var channel = connection.CreateModel ();
    channel.ExchangeDeclare (exchange: queueName,
                         type: ExchangeType.Fanout);
    return channel;
  }
}
