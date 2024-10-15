using System.Runtime.CompilerServices;

namespace AWC.Shared.Kernel.Guards;

public static partial class GuardClauseExtensions
{
    public static DateTime DefaultDateTime
    (
        this IGuardClause guardClause,
        DateTime input,
        string? message = null,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input == default)
        {
            Error(message ?? $"Required input '{parameterName}' is missing.");
        }
        return input;
    }

    public static DateOnly DefaultDateOnly(this IGuardClause guardClause, DateOnly input, string parameterName = "value", string message = null!)
    {
        if (input == default)
        {
            Error(message ?? $"Required input '{parameterName}' is missing.");
        }
        return input;
    }
}
