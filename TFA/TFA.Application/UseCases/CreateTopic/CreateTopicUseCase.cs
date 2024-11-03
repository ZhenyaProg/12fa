using TFA.Application.Authentication;
using TFA.Application.Authorization;
using TFA.Application.Exceptions;
using TFA.Application.Models;

namespace TFA.Application.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IIntentionManager _intentionManager;
    private readonly ICreateTopicStorage _storage;
    private readonly IIdentityProvider _identityProvider;

    public CreateTopicUseCase(IIntentionManager intentionManager,
                              ICreateTopicStorage storage,
                              IIdentityProvider identityProvider)
    {
        _intentionManager = intentionManager;
        _storage = storage;
        _identityProvider = identityProvider;
    }

    public async Task<Topic> Execute(Guid forumId, string title, CancellationToken token)
    {
        _intentionManager.ThrowIfForbidden(TopicIntention.Create);

        bool forumExists = await _storage.ForumExist(forumId, token);
        if(!forumExists)
            throw new ForumNotFoundException(forumId);

        return await _storage.CreateTopic(forumId, _identityProvider.Current.UserId, title, token);
    }
}