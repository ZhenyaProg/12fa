using TFA.Application.Models;

namespace TFA.Application.UseCases.CreateTopic;

public interface ICreateTopicStorage
{
    Task<bool> ForumExist(Guid forumId, CancellationToken cancellationToken);
    Task<Topic> CreateTopic(Guid topicId, Guid userId, string title, CancellationToken cancellationToken);
}