using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics;

namespace RabbitMQFactoryHelpers;
internal class PersistentConnection : IPersistentConnection
{
  private readonly IConnectionFactory connectionFactory;
  private IConnection? _connection;
  private bool _disposed;
  private IConsummerChannel? _eventBusRabbitMq;

  public PersistentConnection ( IConnectionFactory connectionFactory )
  {
    this.connectionFactory = connectionFactory ??
      throw new ArgumentNullException (nameof (connectionFactory));
    if ( !IsConnected )
      TryConnect ();
  }

  public IModel CreateModel ()
  {
    if ( !IsConnected )
      throw new InvalidOperationException ("No RabbitMQ connections are available to perform this action");

    return _connection!.CreateModel ();
  }

  public bool IsConnected =>
    _connection != null && _connection.IsOpen && !_disposed;

  public void Disconnect ()
  {
    if ( !_disposed )
      Dispose ();
  }

  public void Dispose ()
  {
    if ( _disposed )
      return;
    _disposed = true;
    try
    {
      _connection?.Dispose ();
    }
    catch ( Exception exc )
    {
      Debug.WriteLine (exc);
    }
  }
  public bool TryConnect ()
  {
    try
    {
      _connection = connectionFactory.CreateConnection ();
    }
    catch ( BrokerUnreachableException e )
    {
      Debug.WriteLine (e);
      Thread.Sleep (5000);
      _connection = connectionFactory.CreateConnection ();
    }
    if ( !IsConnected )
      return false;

    _connection!.ConnectionShutdown += OnConnectionShutDownAsync;
    _connection!.CallbackException += OnCallbackException;
    _connection!.ConnectionBlocked += OnConnectionBlocked;
    return true;

  }

  private void OnConnectionBlocked ( object? sender, RabbitMQ.Client.Events.ConnectionBlockedEventArgs e )
  {
    if ( _disposed )
      return;
    TryConnect ();
  }

  private void OnCallbackException ( object? sender, RabbitMQ.Client.Events.CallbackExceptionEventArgs e )
  {
    if ( _disposed )
      return;
    TryConnect ();
  }

  private void OnConnectionShutDownAsync ( object? sender, ShutdownEventArgs args )
  {
    if ( _disposed )
      return;
    TryConnect ();
  }

  public void CreateConsumerChannel ( params IConsummerChannel[] channels )
  {
    if ( !IsConnected )
    {
      TryConnect ();
    }

    foreach ( var channel in channels )
    {
      _eventBusRabbitMq = channel;
      _eventBusRabbitMq.SetPersistentConnection (this);
      _eventBusRabbitMq.CreateConsumerChannel ();
    }

  }
}