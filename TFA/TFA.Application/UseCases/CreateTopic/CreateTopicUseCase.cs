using Microsoft.EntityFrameworkCore;
using TFA.Application.Exceptions;
using TFA.Application.Models;
using TFA.Storage;

namespace TFA.Application.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;
    private readonly ForumDbContext _dbContext;

    public CreateTopicUseCase(IGuidFactory guidFactory,
                              IMomentProvider momentProvider,
                              ForumDbContext dbContext)
    {
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
        _dbContext = dbContext;
    }

    public async Task<Topic> Execute(Guid forumId, string title, Guid authorId, CancellationToken token)
    {
        bool forumExists = await _dbContext.Forums.AnyAsync(f => f.Id == forumId, token);
        if(!forumExists)
            throw new ForumNotFoundException(forumId);

        Guid topicId = _guidFactory.Create();
        await _dbContext.Topics.AddAsync(new TopicEntity
        {
            Id = topicId,
            ForumId = forumId,
            AuthorId = authorId,
            Title = title,
            CreatedDate = _momentProvider.Now,
        }, token);
        await _dbContext.SaveChangesAsync(token);

        return await _dbContext.Topics
            .Where(t => t.Id == topicId)
            .Select(t => new Topic
            {
                Id = topicId,
                Title = t.Title,
                CreatedDate = t.CreatedDate,
                Author = t.Author.Login
            })
            .FirstAsync();
    }
}