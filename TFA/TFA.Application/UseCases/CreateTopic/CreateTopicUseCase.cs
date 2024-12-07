using FluentValidation;
using TFA.Application.Authentication;
using TFA.Application.Authorization;
using TFA.Application.Models;
using TFA.Application.UseCases.GetForums;

namespace TFA.Application.UseCases.CreateTopic;

internal class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> _validator;
    private readonly IIntentionManager _intentionManager;
    private readonly ICreateTopicStorage _storage;
    private readonly IGetForumsStorage _getForumsStorage;
    private readonly IIdentityProvider _identityProvider;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        ICreateTopicStorage storage,
        IGetForumsStorage forumsStorage,
        IIdentityProvider identityProvider)
    {
        _validator = validator;
        _intentionManager = intentionManager;
        _storage = storage;
        _getForumsStorage = forumsStorage;
        _identityProvider = identityProvider;
    }

    public async Task<Topic> Execute(CreateTopicCommand command, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(command, token);

        var (forumId, title) = command;
        _intentionManager.ThrowIfForbidden(TopicIntention.Create);

        await _getForumsStorage.ThrowIfForumNotFound(forumId, token);

        return await _storage.CreateTopic(command.ForumId, _identityProvider.Current.UserId, command.Title, token);
    }
}