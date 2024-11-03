using Microsoft.EntityFrameworkCore;
using TFA.Application.Models;
using TFA.Application.UseCases.GetForums;

namespace TFA.Storage.UseCases;

public class GetForumsStorage : IGetForumsStorage
{
    private readonly ForumDbContext _forumDbContext;

    public GetForumsStorage(ForumDbContext forumDbContext)
    {
        _forumDbContext = forumDbContext;
    }

    public async Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken)
    {
        return await _forumDbContext.Forums
            .Select(forum => new Forum
            {
                Id = forum.Id,
                Title = forum.Title,
            })
            .ToArrayAsync(cancellationToken);
    }
}