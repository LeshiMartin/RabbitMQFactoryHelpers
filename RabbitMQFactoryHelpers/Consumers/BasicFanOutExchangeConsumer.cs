using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQFactoryHelpers.Consumers;

internal sealed class BasicFanOutExchangeConsumer : BaseConsumer
{
  public BasicFanOutExchangeConsumer ( string queueName,
    Action<BasicDeliverEventArgs> callBack ) : base (queueName, callBack)
  { }


  public override IModel CreateConsumerChannel ()
  {
    _consumerChannel = CreateConsumerChannel (persistentConnection);
    _consumerChannel.ExchangeDeclare (exchange: queueName, type: ExchangeType.Fanout);

    var _queueName = _consumerChannel.QueueDeclare ().QueueName;
    _consumerChannel.QueueBind (queue: _queueName,
                      exchange: queueName,
                      routingKey: "");
    var consumer = new EventingBasicConsumer (_consumerChannel);
    return FinalizeConsumerChanel (consumer, _consumerChannel);
  }

}
