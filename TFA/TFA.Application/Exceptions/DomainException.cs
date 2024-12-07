namespace TFA.Application.Exceptions;

public abstract class DomainException : Exception
{
    public DomainErrorCodes ErrorCode { get; }

    protected DomainException(DomainErrorCodes errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}