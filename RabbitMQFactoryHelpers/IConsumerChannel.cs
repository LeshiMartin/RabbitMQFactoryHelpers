using RabbitMQ.Client;

namespace RabbitMQFactoryHelpers;
public interface IConsumerChannel : IDisposable
{
  /// <summary>
  /// Creates the consumer channel.
  /// </summary>
  /// <returns></returns>
  IModel CreateConsumerChannel ();


  /// <summary>
  /// Sets the persistent connection.
  /// </summary>
  /// <param name="connection">The connection.</param>
  void SetPersistentConnection ( IPersistentConnection connection );
}
