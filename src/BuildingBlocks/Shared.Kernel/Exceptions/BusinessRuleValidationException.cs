namespace AWC.Shared.Kernel.Exceptions;

public class BusinessRuleValidationException : Exception
{
    public BusinessRuleValidationException(string message, Exception ex) : base(message, ex)
    {
    }

    public BusinessRuleValidationException(string message) : base(message)
    {
    }

    public BusinessRuleValidationException() : base()
    {
    }
}
