using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Application.Models;
using TFA.Application.UseCases.GetForums;

namespace TFA.Storage.UseCases;

internal class GetForumsStorage : IGetForumsStorage
{
    private readonly IMemoryCache _memoryCache;
    private readonly ForumDbContext _forumDbContext;
    private readonly IMapper _mapper;

    public GetForumsStorage(
        IMemoryCache memoryCache,
        ForumDbContext forumDbContext,
        IMapper mapper)
    {
        _memoryCache = memoryCache;
        _forumDbContext = forumDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken)
    {
        Forum[]? forums = await _memoryCache.GetOrCreateAsync<Forum[]>(
                            nameof(GetForums),
                            entry =>
                            {
                                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                                return _forumDbContext.Forums
                                .ProjectTo<Forum>(_mapper.ConfigurationProvider)
                                .ToArrayAsync(cancellationToken);
                            });
        return forums;
    }
}