using RabbitMQ.Client.Events;
using RabbitMQFactoryHelpers.Consumers;

namespace RabbitMQFactoryHelpers.Factories;
public static class ConsumerFactory
{
  /// <summary>
  /// Creates the consumer.
  /// </summary>
  /// <param name="queueName">Name of the queue.</param>
  /// <param name="callBack">The call back.</param>
  /// <returns></returns>
  public static IConsumerChannel CreateConsumer ( string queueName,
    Action<BasicDeliverEventArgs> callBack )
  {
    return new BasicConsumer (queueName, callBack);
  }

  /// <summary>
  /// Creates the fan out exchange consumer.
  /// </summary>
  /// <param name="queueName">Name of the queue.</param>
  /// <param name="callBack">The call back.</param>
  /// <returns></returns>
  public static IConsumerChannel CreateFanOutExchangeConsumer ( string queueName,
    Action<BasicDeliverEventArgs> callBack )
  {
    return new BasicFanOutExchangeConsumer (queueName, callBack);
  }
}
