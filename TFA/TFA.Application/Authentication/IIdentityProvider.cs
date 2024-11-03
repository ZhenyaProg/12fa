namespace TFA.Application.Authentication;

public interface IIdentityProvider
{
    IIdentity Current { get; }
}