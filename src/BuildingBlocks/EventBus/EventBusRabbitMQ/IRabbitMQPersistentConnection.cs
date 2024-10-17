namespace Awc.BuildingBlocks.EventBusRabbitMQ;

public interface IRabbitMQPersistentConnection
    : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();
}
