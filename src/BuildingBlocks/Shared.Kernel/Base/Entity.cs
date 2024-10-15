#pragma warning disable CS8618

namespace AWC.Shared.Kernel.Base;

public abstract class Entity<T>
{
    public T Id { get; protected set; }

    public EntityStatus EntityStatus { get; set; } = EntityStatus.Unmodified;

    public DateTime? ModifiedDate { get; private set; }

    public void UpdateModifiedDate()
    {
        ModifiedDate = DateTime.UtcNow;
    }

    protected virtual void CheckValidity()
    {
        // Validation involving multiple properties go here.
    }
}
