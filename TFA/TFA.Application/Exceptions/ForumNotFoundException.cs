namespace TFA.Application.Exceptions;

public class ForumNotFoundException : DomainException
{
    public ForumNotFoundException(Guid forumId) :
        base(DomainErrorCodes.Gone, $"Forum with id {forumId} was not found")
    {
        
    }
}