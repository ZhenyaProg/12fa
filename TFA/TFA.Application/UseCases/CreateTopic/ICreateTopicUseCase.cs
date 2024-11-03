using TFA.Application.Models;

namespace TFA.Application.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> Execute(Guid forumId, string title, CancellationToken token);
}