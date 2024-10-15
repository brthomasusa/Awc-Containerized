using System.Runtime.CompilerServices;

namespace AWC.Shared.Kernel.Guards;

public static partial class GuardClauseExtensions
{
    public static string NullOrEmpty
    (
        this IGuardClause guardClause,
        string input,
        string? message = null,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (string.IsNullOrEmpty(input))
        {
            Error(message ?? $"Required input '{parameterName}' is missing.");
        }
        return input!;
    }

    public static object Null
    (
        this IGuardClause guardClause,
        object input,
        string? message = null,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input is null)
        {
            Error(message ?? $"Required input '{parameterName}' is missing.");
        }
        return input!;
    }
}
