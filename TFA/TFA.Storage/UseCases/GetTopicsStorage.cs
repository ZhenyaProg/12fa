using Microsoft.EntityFrameworkCore;
using TFA.Application.Models;
using TFA.Application.UseCases.GetTopics;

namespace TFA.Storage.UseCases;

internal class GetTopicsStorage : IGetTopicsStorage
{
    private readonly ForumDbContext _dbContext;

    public GetTopicsStorage(
        ForumDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken token)
    {
        var query = _dbContext.Topics.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync();
        var resources = await query
            .Where(t => t.ForumId == forumId)
            .Select(t => new Topic
            {
                Id = t.ForumId,
                ForumId = t.ForumId,
                AuthorId = t.AuthorId,
                Title = t.Title,
                CreatedDate = t.CreatedDate,
            })
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(token);

        return (resources, totalCount);
    }
}