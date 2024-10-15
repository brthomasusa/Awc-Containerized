namespace AWC.Shared.Kernel.Base;

public abstract class EntityStatus(int value, string name) : Enumeration<EntityStatus>(value, name)
{
    public static readonly EntityStatus Unmodified = new UnmodifiedStatus();
    public static readonly EntityStatus Added = new AddedStatus();
    public static readonly EntityStatus Modified = new ModifiedStatus();
    public static readonly EntityStatus Deleted = new DeletedStatus();

    private sealed class UnmodifiedStatus : EntityStatus
    {
        public UnmodifiedStatus()
            : base(1, "Unmodified")
        { }
    }

    private sealed class AddedStatus : EntityStatus
    {
        public AddedStatus()
            : base(2, "Added")
        { }
    }

    private sealed class ModifiedStatus : EntityStatus
    {
        public ModifiedStatus()
            : base(3, "Modified")
        { }
    }

    private sealed class DeletedStatus : EntityStatus
    {
        public DeletedStatus()
            : base(4, "Deleted")
        { }
    }
}
