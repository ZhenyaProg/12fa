namespace TFA.Application.Authentication;

internal class IdentityProvider : IIdentityProvider
{
    public IIdentity Current => new User(Guid.Parse("459ec210-bb46-4729-80bc-35a09706db2c"));
}