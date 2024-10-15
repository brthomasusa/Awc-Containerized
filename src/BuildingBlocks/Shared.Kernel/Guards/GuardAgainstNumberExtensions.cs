using System.Runtime.CompilerServices;

namespace AWC.Shared.Kernel.Guards;

public static partial class GuardClauseExtensions
{
    public static int LessThan
    (
        this IGuardClause guardClause,
        int input,
        int minValue,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input < minValue)
        {
            Error(message ?? $"'{parameterName}' must be greater than {minValue}.");
        }
        return input;
    }

    public static decimal LessThan
    (
        this IGuardClause guardClause,
        decimal input,
        decimal minValue,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input < minValue)
        {
            Error(message ?? $"'{parameterName}' must be greater than {minValue}.");
        }
        return input;
    }

    public static int GreaterThan
    (
        this IGuardClause guardClause,
        int input,
        int maxValue,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input > maxValue)
        {
            Error(message ?? $"'{parameterName}' must be less than {maxValue}.");
        }
        return input;
    }

    public static decimal GreaterThan
    (
        this IGuardClause guardClause,
        decimal input,
        decimal maxValue,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input > maxValue)
        {
            Error(message ?? $"'{parameterName}' must be less than {maxValue}.");
        }
        return input;
    }

    public static decimal GreaterThanTwoDecimalPlaces
    (
        this IGuardClause guardClause,
        decimal input,
        string message = null!,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null
    )
    {
        if (input % 0.01M != 0)
        {
            Error(message ?? $"'{parameterName}' is limited to two decimal places.");
        }
        return input;
    }
}
