#pragma warning disable CS8601, CS8603, CS8618

namespace Awc.BuildingBlocks.IntegrationEventLogEF;

public class IntegrationEventLogEntry
{
    private IntegrationEventLogEntry() { }
    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName;                     
        Content = JsonSerializer.Serialize(@event, @event.GetType(), s_writeOptions);
        State = EventStateEnum.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId.ToString();
    }
    public Guid EventId { get; }
    public string EventTypeName { get; }
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.')?.Last();
    [NotMapped]
    public IntegrationEvent IntegrationEvent { get; private set; }
    public EventStateEnum State { get; set; }
    public int TimesSent { get; set; }
    public DateTime CreationTime { get; }
    public string Content { get; }
    public string TransactionId { get; }

    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {            
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, s_writeOptions) as IntegrationEvent;
        return this;
    }
    private static readonly JsonSerializerOptions s_writeOptions = new()
    {
        WriteIndented = true
    };
    
}