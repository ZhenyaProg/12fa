using Microsoft.EntityFrameworkCore;
using TFA.Application.Models;
using TFA.Storage;

namespace TFA.Application.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly ForumDbContext _dbContext;

    public GetForumsUseCase(ForumDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Forum>> Execute(CancellationToken token)
    {
        return await _dbContext.Forums
            .Select(f => new Forum
            {
                Id = f.Id,
                Title = f.Title,
            })
            .ToArrayAsync(token);
    }
}