using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQFactoryHelpers.Consumers;

internal sealed class BasicConsumer : BaseConsumer
{
    public BasicConsumer(string queueName,
        Action<BasicDeliverEventArgs> callBack) : base(queueName, callBack)
    {
    }

    public override IModel CreateConsumerChannel()
    {
        _consumerChannel = CreateConsumerChannel(persistentConnection);
        _consumerChannel.QueueDeclare(queueName, false, false, false, null);
        var consumer = new EventingBasicConsumer(_consumerChannel);
        return FinalizeConsumerChanel(consumer, _consumerChannel);
    }
}