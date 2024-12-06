using FluentValidation;
using TFA.Application.Authentication;
using TFA.Application.Authorization;
using TFA.Application.Exceptions;
using TFA.Application.Models;

namespace TFA.Application.UseCases.CreateTopic;

internal class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> _validator;
    private readonly IIntentionManager _intentionManager;
    private readonly ICreateTopicStorage _storage;
    private readonly IIdentityProvider _identityProvider;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        ICreateTopicStorage storage,
        IIdentityProvider identityProvider)
    {
        _validator = validator;
        _intentionManager = intentionManager;
        _storage = storage;
        _identityProvider = identityProvider;
    }

    public async Task<Topic> Execute(CreateTopicCommand command, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(command, token);

        _intentionManager.ThrowIfForbidden(TopicIntention.Create);

        bool forumExists = await _storage.ForumExist(command.ForumId, token);
        if(!forumExists)
            throw new ForumNotFoundException(command.ForumId);

        return await _storage.CreateTopic(command.ForumId, _identityProvider.Current.UserId, command.Title, token);
    }
}