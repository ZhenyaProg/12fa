using TFA.Application.Models;

namespace TFA.Application.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(
        Guid forumId, int skip, int take, CancellationToken token);
}