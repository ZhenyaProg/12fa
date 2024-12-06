using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TFA.Application.Authentication;
using TFA.Application.Authorization;
using TFA.Application.Models;
using TFA.Application.UseCases.CreateTopic;
using TFA.Application.UseCases.GetForums;

namespace TFA.Application.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForumDomain(this IServiceCollection services)
        {
            return services.AddScoped<IGetForumsUseCase, GetForumsUseCase>()
                           .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
                           .AddScoped<IIntentionResolver, TopicIntentionResolver>()
                           .AddScoped<IIntentionManager, IntentionManager>()
                           .AddScoped<IIdentityProvider, IdentityProvider>()
                           .AddValidatorsFromAssemblyContaining<Forum>(includeInternalTypes: true);
        }
    }
}