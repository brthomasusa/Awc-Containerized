
namespace AWC.Shared.Kernel.Guards;

public interface IGuardClause;

public sealed class Guard : IGuardClause
{
    public static IGuardClause Against { get; } = new Guard();

    private Guard() { }
}
