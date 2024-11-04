namespace TFA.Application.Exceptions;

public class ForumNotFoundException : DomainException
{
    public ForumNotFoundException(Guid forumId) :
        base(ErrorCodes.Gone, $"Forum with id {forumId} was not found")
    {
        
    }
}