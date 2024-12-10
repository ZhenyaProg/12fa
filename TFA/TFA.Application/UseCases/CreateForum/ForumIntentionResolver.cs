using TFA.Application.Authentication;
using TFA.Application.Authorization;

namespace TFA.Application.UseCases.CreateForum
{
    internal class ForumIntentionResolver : IIntentionResolver<ForumIntention>
    {
        public bool IsAllowed(IIdentity subject, ForumIntention intention)
        {
            return intention switch
            {
                ForumIntention.Create => subject.IsAuthenticated(),
                _ => false,
            };
        }
    }
}