using TFA.Application.Authentication;

namespace TFA.Application.Authorization;

public interface IIntentionManager
{
    bool IsAllowed<TIntetion>(TIntetion intetion) where TIntetion : struct;
    bool IsAllowed<TIntetion, TObject>(TIntetion intetion, TObject target) where TIntetion : struct;
}

internal class IntentionManager : IIntentionManager
{
    private readonly IEnumerable<IIntentionResolver> _resolvers;
    private readonly IIdentityProvider _identityProvider;

    public IntentionManager(IEnumerable<IIntentionResolver> resolvers, IIdentityProvider identityProvider)
    {
        _resolvers = resolvers;
        _identityProvider = identityProvider;
    }

    public bool IsAllowed<TIntetion>(TIntetion intetion) where TIntetion : struct
    {
        var mathcingResolver = _resolvers.OfType<IIntentionResolver<TIntetion>>().FirstOrDefault();
        return mathcingResolver?.IsAllowed(_identityProvider.Current, intetion) ?? false;
    }

    public bool IsAllowed<TIntetion, TObject>(TIntetion intetion, TObject target) where TIntetion : struct
    {
        throw new NotImplementedException();
    }
}

internal static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention))
            throw new IntentionManagerException();
    }
}