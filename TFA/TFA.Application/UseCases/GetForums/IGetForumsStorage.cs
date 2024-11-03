using TFA.Application.Models;

namespace TFA.Application.UseCases.GetForums;

public interface IGetForumsStorage
{
    Task<IEnumerable<Forum>> GetForums(CancellationToken cancellationToken);
}