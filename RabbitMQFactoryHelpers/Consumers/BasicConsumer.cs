using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQFactoryHelpers.Consumers;

internal sealed class BasicConsumer : BaseConsumer
{
  public BasicConsumer ( string queueName,
    Action<BasicDeliverEventArgs> callBack ) : base (queueName, callBack)
  { }
  public override IModel CreateConsumerChannel ()
  {
    _consumerChannel = CreateConsumerChannel (persistentConnection);
    _consumerChannel.QueueDeclare (queue: queueName,
      durable: false,
      exclusive: false,
      autoDelete: false,
      arguments: null);

    var consumer = new EventingBasicConsumer (_consumerChannel);
    return FinalizeConsumerChanel (consumer, _consumerChannel);
  }
}
