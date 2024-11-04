namespace TFA.Application.Exceptions;

public abstract class DomainException : Exception
{
    public ErrorCodes ErrorCode { get; }

    protected DomainException(ErrorCodes errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}