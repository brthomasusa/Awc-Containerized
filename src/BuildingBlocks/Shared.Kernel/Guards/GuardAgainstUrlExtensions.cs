using System.Runtime.CompilerServices;

namespace AWC.Shared.Kernel.Guards;

public static partial class GuardClauseExtensions
{
    public static string InvalidUrl
    (
        this IGuardClause guardClause,
        string input,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        try
        {
            _ = new Uri(input);
        }
        catch
        {
            Error(message ?? $"{parameterName} is not a valid URL.");
        }
        return input;
    }
}
