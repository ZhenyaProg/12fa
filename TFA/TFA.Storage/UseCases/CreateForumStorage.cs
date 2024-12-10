using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateForum;

namespace TFA.Storage.UseCases;

internal class CreateForumStorage : ICreateForumStorage
{
    private readonly IMemoryCache _memoryCache;
    private readonly IGuidFactory _guidFactory;
    private readonly ForumDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateForumStorage(
        IMemoryCache memoryCache,
        IGuidFactory guidFactory,
        ForumDbContext dbContext,
        IMapper mapper)
    {
        _memoryCache = memoryCache;
        _guidFactory = guidFactory;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Forum> Create(string title, CancellationToken cancellationToken)
    {
        Guid forumId = _guidFactory.Create();
        var forum = new ForumEntity
        {
            Id = forumId,
            Title = title,
        };

        await _dbContext.Forums.AddAsync(forum);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(nameof(GetForumsStorage.GetForums));

        return await _dbContext.Forums
            .Where(f => f.Id == forumId)
            .ProjectTo<Forum>(_mapper.ConfigurationProvider)
            .FirstAsync();
    }
}