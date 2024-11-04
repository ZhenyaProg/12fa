using TFA.Application.Models;

namespace TFA.Application.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> Execute(CreateTopicCommand command, CancellationToken token);
}