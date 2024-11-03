using TFA.Application.Authentication;
using TFA.Application.Authorization;

namespace TFA.Application.UseCases.CreateTopic;

public class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention)
    {
        return intention switch
        {
            TopicIntention.Create => subject.IsAuthenticated(),
            _ => false,
        };
    }
}