using TFA.Application.Models;

namespace TFA.Application.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IGetForumsStorage _storage;

    public GetForumsUseCase(IGetForumsStorage storage)
    {
        _storage = storage;
    }

    public async Task<IEnumerable<Forum>> Execute(CancellationToken token)
    {
        return await _storage.GetForums(token);
    }
}