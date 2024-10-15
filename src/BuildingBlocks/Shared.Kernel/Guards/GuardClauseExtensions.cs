using AWC.Shared.Kernel.Exceptions;

namespace AWC.Shared.Kernel.Guards;

public static partial class GuardClauseExtensions
{
    private static void Error(string message)
    {
        throw new DomainException(message);
    }
}
