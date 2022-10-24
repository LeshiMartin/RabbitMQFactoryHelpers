using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQFactoryHelpers.Consumers;

internal abstract class BaseConsumer : IConsumerChannel
{
    protected BaseConsumer(string queueName, Action<BasicDeliverEventArgs> callBack)
    {
        this.queueName = queueName;
        CallBack = callBack;
    }

    protected IPersistentConnection? persistentConnection;
    protected IModel? _consumerChannel;
    protected readonly string queueName;
    protected Action<BasicDeliverEventArgs> CallBack { get; set; }
    public abstract IModel CreateConsumerChannel();

    protected IModel CreateConsumerChannel(IPersistentConnection? persistantConnection)
    {
        if (!persistantConnection!.IsConnected)
            persistantConnection.TryConnect();
        _consumerChannel = persistantConnection.CreateModel();
        return _consumerChannel;
    }

    private void ReceivedEvent(object? sender, BasicDeliverEventArgs ea)
        => CallBack(ea);


    public void SetPersistentConnection(IPersistentConnection connection)
    {
        persistentConnection = connection;
    }

    protected IModel FinalizeConsumerChanel(EventingBasicConsumer consumer, IModel consumerChannel)
    {
        consumer.Received += ReceivedEvent;
        consumerChannel.BasicConsume(queueName, true, consumer);
        consumerChannel.CallbackException += (_, _) => consumerChannel.Dispose();
        return consumerChannel;
    }
    

    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected  void Dispose(bool disposing)
    {
        if (_isDisposed) return;
        if (disposing)
        {
            _consumerChannel?.Dispose();
        }
        _isDisposed = true;
    }

    
}