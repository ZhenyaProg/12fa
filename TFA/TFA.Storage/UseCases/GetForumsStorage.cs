using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Application.Models;
using TFA.Application.UseCases.GetForums;

namespace TFA.Storage.UseCases;

internal class GetForumsStorage : IGetForumsStorage
{
    private readonly IMemoryCache _memoryCache;
    private readonly ForumDbContext _forumDbContext;

    public GetForumsStorage(
        IMemoryCache memoryCache,
        ForumDbContext forumDbContext)
    {
        _memoryCache = memoryCache;
        _forumDbContext = forumDbContext;
    }

    public async Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync<Forum[]>(
            nameof(GetForums),
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return _forumDbContext.Forums
                .Select(f => new Forum
                {
                    Id = f.Id,
                    Title = f.Title,
                })
                .ToArrayAsync(cancellationToken);
            });
    }
}