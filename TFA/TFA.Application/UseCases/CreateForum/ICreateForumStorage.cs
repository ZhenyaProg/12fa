using TFA.Application.Models;

namespace TFA.Application.UseCases.CreateForum;

public interface ICreateForumStorage
{
    public Task<Forum> Create(string title, CancellationToken cancellationToken);
}