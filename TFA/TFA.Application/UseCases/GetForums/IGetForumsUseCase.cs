using TFA.Application.Models;

namespace TFA.Application.UseCases.GetForums
{
    public interface IGetForumsUseCase
    {
        Task<IEnumerable<Forum>> Execute(CancellationToken token);
    }
}