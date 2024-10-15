namespace AWC.Shared.Kernel.Utilities;

public class Error(string code, string message) : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met.");

    public string Code { get; } = code;

    public string Message { get; } = message;

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b) => (a is null && b is null) || (a is not null && b is not null && a.Equals(b));

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        return other is not null && Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => Code;
}
