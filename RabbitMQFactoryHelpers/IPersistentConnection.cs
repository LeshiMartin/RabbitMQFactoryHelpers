using RabbitMQ.Client;

namespace RabbitMQFactoryHelpers;

public interface IPersistentConnection
{
  bool IsConnected { get; }

  bool TryConnect ();

  IModel CreateModel ();

  void CreateConsumerChannel ( params IConsumerChannel[] channels );

  void Disconnect ();
}
