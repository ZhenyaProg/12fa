using TFA.Application.Authentication;

namespace TFA.Application.Authorization;

public interface IIntentionResolver
{
}

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}