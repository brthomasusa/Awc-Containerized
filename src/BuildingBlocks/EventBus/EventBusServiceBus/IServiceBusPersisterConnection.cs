namespace Awc.BuildingBlocks.EventBusServiceBus;

public interface IServiceBusPersisterConnection : IAsyncDisposable
{
    ServiceBusClient TopicClient { get; }
    ServiceBusAdministrationClient AdministrationClient { get; }
}