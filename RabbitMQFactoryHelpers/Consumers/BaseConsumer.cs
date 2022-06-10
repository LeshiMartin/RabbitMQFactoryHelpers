using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQFactoryHelpers.Consumers;

internal abstract class BaseConsumer : IConsummerChannel
{
  protected BaseConsumer ( string queueName, Action<BasicDeliverEventArgs> callBack )
  {
    this.queueName = queueName;
    CallBack = callBack;
  }
  protected IPersistentConnection? persistentConnection;
  protected IModel? _consumerChannel;
  protected readonly string queueName;
  protected Action<BasicDeliverEventArgs> CallBack { get; set; }
  public abstract IModel CreateConsumerChannel ();

  protected IModel CreateConsumerChannel ( IPersistentConnection? persistantConnection )
  {
    if ( !persistantConnection!.IsConnected )
      persistantConnection.TryConnect ();
    _consumerChannel = persistantConnection.CreateModel ();
    return _consumerChannel;
  }
  private void ReceivedEvent ( object? sender, BasicDeliverEventArgs ea )
    => CallBack (ea);


  public void SetPersistentConnection ( IPersistentConnection connection )
  {
    persistentConnection = connection;
  }

  protected IModel FinalizeConsumerChanel ( EventingBasicConsumer consumer, IModel consummerChannel )
  {
    consumer.Received += ReceivedEvent;
    consummerChannel.BasicConsume (queue: queueName, autoAck: true, consumer: consumer);
    consummerChannel.CallbackException += ( sender, ea ) =>
    {
      consummerChannel.Dispose ();
    };
    return consummerChannel;
  }



  #region Dispose

  private bool _isDisposed;

  public void Dispose ()
  {
    Dispose (true);
    GC.SuppressFinalize (this);
  }

  protected virtual void Dispose ( bool disposing )
  {
    if ( !_isDisposed )
    {
      if ( disposing )
      {
        _consumerChannel?.Dispose ();
      }
      _isDisposed = true;
    }
  }
  #endregion
}
