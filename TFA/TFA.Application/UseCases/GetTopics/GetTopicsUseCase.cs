using FluentValidation;
using TFA.Application.Models;
using TFA.Application.UseCases.GetForums;

namespace TFA.Application.UseCases.GetTopics;

internal class GetTopicsUseCase : IGetTopicsUseCase
{
    private readonly IValidator<GetTopicsQuery> _validator;
    private readonly IGetTopicsStorage _storage;
    private readonly IGetForumsStorage _getForumsStorage;

    public GetTopicsUseCase(
        IValidator<GetTopicsQuery> validator,
        IGetTopicsStorage storage,
        IGetForumsStorage getForumsStorage)
    {
        _validator = validator;
        _storage = storage;
        _getForumsStorage = getForumsStorage;
    }

    public async Task<(IEnumerable<Topic> resources, int totalCount)> Execute(
        GetTopicsQuery query, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(query, token);
        await _getForumsStorage.ThrowIfForumNotFound(query.ForumId, token);
        return await _storage.GetTopics(query.ForumId, query.Skip, query.Take, token);
    }
}