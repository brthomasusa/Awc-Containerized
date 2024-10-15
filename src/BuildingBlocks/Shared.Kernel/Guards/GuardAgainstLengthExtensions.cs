using System.Runtime.CompilerServices;

namespace AWC.Shared.Kernel.Guards;

public static partial class GuardClauseExtensions
{
    public static string LengthGreaterThan
    (
        this IGuardClause guardClause,
        string input,
        int maxLength,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input.Length > maxLength)
        {
            Error(message ?? $"'{parameterName}' length must be less than or equal to {maxLength} characters.");
        }
        return input;
    }
}
