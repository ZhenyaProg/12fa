using TFA.Application.Models;

namespace TFA.Application.UseCases.GetTopics;

public interface IGetTopicsUseCase
{
    Task<(IEnumerable<Topic> resources, int totalCount)> Execute(
        GetTopicsQuery query, CancellationToken token);
}