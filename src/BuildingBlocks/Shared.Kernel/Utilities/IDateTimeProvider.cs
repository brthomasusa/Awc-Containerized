namespace AWC.Shared.Kernel.Utilities;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
