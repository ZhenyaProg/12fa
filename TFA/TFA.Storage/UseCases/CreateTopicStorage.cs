using Microsoft.EntityFrameworkCore;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateTopic;

namespace TFA.Storage.UseCases;

internal class CreateTopicStorage : ICreateTopicStorage
{
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;
    private readonly ForumDbContext _forumDbContext;

    public CreateTopicStorage(IGuidFactory guidFactory,
                              IMomentProvider momentProvider,
                              ForumDbContext forumDbContext)
    {
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
        _forumDbContext = forumDbContext;
    }

    public async Task<Topic> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken)
    {
        Guid topicId = _guidFactory.Create();
        var topic = new TopicEntity
        {
            Id = topicId,
            ForumId = forumId,
            AuthorId = userId,
            Title = title,
            CreatedDate = _momentProvider.Now,
        };

        await _forumDbContext.Topics.AddAsync(topic, cancellationToken);
        await _forumDbContext.SaveChangesAsync();

        return await _forumDbContext.Topics
            .Where(t => t.Id == topicId)
            .Select(t => new Topic
            {
                Id = t.Id,
                ForumId = t.ForumId,
                AuthorId = t.AuthorId,
                Title = t.Title,
                CreatedDate = t.CreatedDate,
            })
            .FirstAsync(cancellationToken);
    }

    public async Task<bool> ForumExist(Guid forumId, CancellationToken cancellationToken) =>
        await _forumDbContext.Forums.AnyAsync(f => f.Id == forumId, cancellationToken);
}