
namespace AWC.Shared.Kernel.Base;

public abstract class IntegrationEvent(Guid id)
{
    protected IntegrationEvent() : this(Guid.NewGuid())
    {
        CreatedDate = DateTime.Now;
    }

    public readonly Guid Id = id;
    public readonly DateTime CreatedDate = DateTime.Now;
}
